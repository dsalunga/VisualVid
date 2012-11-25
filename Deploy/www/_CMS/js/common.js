function CheckAll(chk, chk_item)
		{
				var i, chk_items, isChecked;
				
				chk_items = document.all.item(chk_item);
				isChecked = chk.checked;
				
				if(chk_items == null)
				{
					return;
				}
				
				if(chk_items.length > 0)
				{
					// MULTIPLE ITEMS
					for(i=0; i<chk_items.length; i++)
					{
						chk_items[i].checked = isChecked;
					}
				}
				else
				{
					// ONE ITEM ONLY
					chk_items.checked = isChecked;
				}
			}


function Upload(cntElement, sUploadFolder, sArgs)
			{
				// s_args:
				// _f=true ->> return only filename
				// _filename=dest_filename (without extension) ->> uses the provided filename when saving
					
				var left = (screen.availWidth/2) - (400/2);
				var top = (screen.availHeight/2) - (300/2);
				window.open('Utils/Upload.aspx?UploadFolder=' + sUploadFolder + '&Control=' + cntElement + sArgs,'FileManager','width=400,height=300,left='+left+',top='+top);
			}
			
function BrowseLink(cntElement)
			{
				var left = (screen.availWidth/2) - (400/2);
				var top = (screen.availHeight/2) - (300/2);
				window.open('Utils/LinkBrowser.aspx?Control=' + cntElement,'LinkBrowser','resizable=1,scrollbars=1,width=400,height=300,left=' + left + ',top=' + top);
			}
			
			function ShowDelete()
			{
			    return confirm('Are sure you want to delete?');
			}