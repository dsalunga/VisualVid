using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading;
using System.Text;
using System.Xml;

using DES;

namespace DES.VisualVid.Encoder
{
    class Program
    {
        static void Main(string[] args)
        {
            bool hasRows = true;
            while (true)
            {
                Process[] ms = Process.GetProcessesByName("ffmpeg");
                if (ms.Length > 0)
                {
                    // foreach (Process m in ms){}
                    Thread.Sleep(30000); // Wait for 30 seconds
                    Console.WriteLine("Encoder running. Wait for 30 seconds...");
                }
                else
                {
                    /* Not running */
                    /* Check for pending files for encoding... */
                    Console.WriteLine("Begin query for pending video files");

                    using (SqlDataReader r = SqlHelper.ExecuteReader("SELECT_Videos_Pending"))
                    {
                        hasRows = r.HasRows;

                        while (r.Read())
                        {
                            //int iVideoID = Convert.ToInt32(r["VideoId"]);
                            string sVideoId = r["VideoId"].ToString();
                            string sExt = r["OriginalExtension"].ToString();
                            string sUserId = r["UserId"].ToString();
                            string sEmail = r["Email"].ToString();
                            string sTitle = r["Title"].ToString();

                            string sAppPath = Environment.CurrentDirectory;
                            string sVideoPath = Directory.GetParent(sAppPath).FullName;
                            //string sSrcFilename = sUniqueID + sExt;
                            //string sDesFilename = sUniqueID + ".flv";

                            string sSrcPath = string.Format(@"{0}\Pending\{1}{2}", sVideoPath, sVideoId, sExt);
                            string sDesPath = string.Format(@"{0}\Members\{1}\{2}.flv", sVideoPath, sUserId, sVideoId);
                            string sDesPathImage = string.Format(@"{0}\Members\{1}\{2}.jpg", sVideoPath, sUserId, sVideoId);

                            using (XmlReader xr = XmlReader.Create(sAppPath + (@"\VideoEncoders.xml")))
                            {
                                Console.WriteLine("Pending video(s) found. Begin read on VideoEncoders.xml");

                                xr.Read();
                                xr.ReadToNextSibling("Encoders");
                                xr.ReadToDescendant("VideoType");

                                while (!xr.EOF)
                                {
                                    Console.WriteLine("Finding appropriate encoder...");

                                    if (xr["Extension"].ToLower().IndexOf(sExt) > -1 || xr["Extension"] == ".*")
                                    {
                                        Console.WriteLine("Encoder found for {0}", sExt);

                                        try
                                        {
                                            xr.ReadToDescendant("VideoExecutable");
                                            string sVideoExec = sAppPath + "\\" + xr.ReadString();
                                            xr.ReadToNextSibling("VideoParameters");
                                            string sVideoParams = string.Format(xr.ReadString(), sSrcPath, sDesPath);

                                            xr.ReadToNextSibling("ImageExecutable");
                                            string sImageExec = sAppPath + "\\" + xr.ReadString();
                                            xr.ReadToNextSibling("ImageParameters");
                                            string sImageParams = string.Format(xr.ReadString(), sSrcPath, sDesPathImage); // xr.ReadString();

                                            Console.WriteLine("Video: {0} {1}\nImage: {2} {3}\n\nStarting processes...", sVideoExec, sVideoParams, sImageExec, sImageParams);
                                            //Console.ReadLine();

                                            // Generate Thumbnail
                                            using (Process imageProcess = new Process())
                                            {
                                                //imageProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                                //imageProcess.StartInfo.RedirectStandardError = true;
                                                imageProcess.StartInfo.Arguments = sImageParams;
                                                imageProcess.StartInfo.FileName = sImageExec;
                                                imageProcess.Start();
                                                imageProcess.PriorityClass = ProcessPriorityClass.BelowNormal;
                                                imageProcess.WaitForExit();
                                            }

                                            // Encode Video
                                            using (Process videoProcess = new Process())
                                            {
                                                //videoProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                                //videoProcess.StartInfo.RedirectStandardError = true;
                                                videoProcess.StartInfo.Arguments = sVideoParams;
                                                videoProcess.StartInfo.FileName = sVideoExec;
                                                videoProcess.Start();
                                                videoProcess.PriorityClass = ProcessPriorityClass.BelowNormal;
                                                videoProcess.WaitForExit();
                                            }

                                            // Add flv tags
                                            using (Process flvtoolProcess = new Process())
                                            {
                                                //flvtoolProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                                //flvtoolProcess.StartInfo.RedirectStandardError = true;
                                                flvtoolProcess.StartInfo.Arguments = string.Format("u -n \"{0}\"", sDesPath);
                                                flvtoolProcess.StartInfo.FileName = sAppPath + "\\flvtool2.exe";
                                                flvtoolProcess.Start();
                                                flvtoolProcess.PriorityClass = ProcessPriorityClass.BelowNormal;
                                                flvtoolProcess.WaitForExit();
                                            }

                                            SqlHelper.ExecuteNonQuery(CommandType.Text, "UPDATE Videos SET Pending=0, IsActive=1 WHERE VideoId=@VideoId",
                                                new SqlParameter("@VideoId", new Guid(sVideoId))
                                            );

                                            // Delete temporary file.
                                            File.Delete(sSrcPath);

                                            // Send email
                                            string sBody = ConfigurationManager.AppSettings["Email_Body"]; /* "<font face='Tahoma' size='2'><p>Dear&nbsp;<strong>{0}</strong>,</p><p>Thank you for submitting your video to VisualVid. " +
                                                "Your video is now posted at the VisualVid website, you can view it by clicking the link or by copying the address and paste it in your browser." +
                                                "<br /><br /><br />Click or copy the link below into your browser to view your video:<br /><a href='{1}' title='Click here to view your video'>{1}</a><br /><br /><br />" +
                                                "</p><p><strong>The VisualVid Team</strong><br /><a title='visit VisualVid.com' href='{2}'>{2}</a></p></font>"; */

                                            string sVideoLink = ConfigurationManager.AppSettings["Email_Video_Link"] + sVideoId;

                                            MailMessage mail = new MailMessage();
                                            //mail.From = new MailAddress(ConfigurationManager.AppSettings["Email_From")
                                            mail.To.Add(new MailAddress(sEmail));
                                            mail.Subject = ConfigurationManager.AppSettings["Email_Subject"] + sTitle;
                                            mail.IsBodyHtml = true;
                                            mail.Body = string.Format(sBody, sEmail, sVideoLink, ConfigurationManager.AppSettings["VisualVid_Home"]);

                                            SmtpClient client = new SmtpClient();
                                            client.Send(mail);
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine("Error: " + ex.Message);
                                            //Console.ReadLine();

                                            SqlHelper.ExecuteNonQuery(CommandType.Text,
                                                "DELETE FROM Videos WHERE VideoId=@VideoId",
                                                new SqlParameter("@VideoId", new Guid(sVideoId))
                                            );

                                            // Send email
                                            string sBody = ConfigurationManager.AppSettings["Email_Body_Failed"]; /* "<font face='Tahoma' size='2'><p>Dear&nbsp;<strong>{0}</strong>,</p><p>Thank you for submitting your video to VisualVid. " +
                                                "Your video is now posted at the VisualVid website, you can view it by clicking the link or by copying the address and paste it in your browser." +
                                                "<br /><br /><br />Click or copy the link below into your browser to view your video:<br /><a href='{1}' title='Click here to view your video'>{1}</a><br /><br /><br />" +
                                                "</p><p><strong>The VisualVid Team</strong><br /><a title='visit VisualVid.com' href='{2}'>{2}</a></p></font>"; */

                                            //string sVideoLink = ConfigurationManager.AppSettings["Email_Video_Link"] + sVideoId;

                                            // Delete temporary file.
                                            File.Delete(sSrcPath);

                                            MailMessage mail = new MailMessage();
                                            //mail.From = new MailAddress(ConfigurationManager.AppSettings["Email_From")
                                            mail.To.Add(new MailAddress(sEmail));
                                            mail.Subject = ConfigurationManager.AppSettings["Email_Subject_Failed"] + sTitle;
                                            mail.IsBodyHtml = true;
                                            mail.Body = string.Format(sBody, sEmail, ConfigurationManager.AppSettings["VisualVid_Home"]);

                                            SmtpClient client = new SmtpClient();
                                            try
                                            {

                                                client.Send(mail);
                                            }
                                            catch (Exception ex1)
                                            {
                                                Console.WriteLine("Error: " + ex1.Message);
                                            }
                                        }

                                        Console.WriteLine("1 file successfully encoded.");
                                        break;
                                    }
                                    else
                                    {
                                        //xr.ReadEndElement();
                                        xr.ReadToNextSibling("VideoType");
                                    }
                                }
                            }
                        }
                    }


                    if (!hasRows)
                    {
                        Console.WriteLine("End of encoding. Exiting process.");
                        //Console.ReadLine();
                        break;
                    }
                }
            }
        }
    }
}