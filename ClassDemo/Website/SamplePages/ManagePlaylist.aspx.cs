using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additional Namespaces
using ChinookSystem.BLL;
using Chinook.Data.POCOs;

#endregion
public partial class SamplePages_ManagePlaylist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Request.IsAuthenticated)
        {
            Response.Redirect("~/Account/Login.aspx");
        }
        else
        {
            TracksSelectionList.DataSource = null;
        }
    }

    protected void CheckForException(object sender, ObjectDataSourceStatusEventArgs e)
    {
        MessageUserControl.HandleDataBoundException(e);
    }

    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        //PreRenderComplete occurs just after databinding page events
        //load a pointer to point to your DataPager control
        DataPager thePager = TracksSelectionList.FindControl("DataPager1") as DataPager;
        if (thePager !=null)
        {
            //this code will check the StartRowIndex to see if it is greater that the
            //total count of the collection
            if (thePager.StartRowIndex > thePager.TotalRowCount)
            {
                thePager.SetPageProperties(0, thePager.MaximumRows, true);
            }
        }
    }

    protected void ArtistFetch_Click(object sender, EventArgs e)
    {
        //code to go here
        TracksBy.Text = "Artist";
        SearchArgID.Text = ArtistDDL.SelectedValue;
        //refresh the track list display
        TracksSelectionList.DataBind();
    }

    protected void MediaTypeFetch_Click(object sender, EventArgs e)
    {
        //code to go here
        TracksBy.Text = "MediaType";
        SearchArgID.Text = MediaTypeDDL.SelectedValue;
        //refresh the track list display
        TracksSelectionList.DataBind();
    }

    protected void GenreFetch_Click(object sender, EventArgs e)
    {
        //code to go here
        TracksBy.Text = "Genre";
        SearchArgID.Text = GenreDDL.SelectedValue;
        //refresh the track list display
        TracksSelectionList.DataBind();
    }

    protected void AlbumFetch_Click(object sender, EventArgs e)
    {
        //code to go here
        TracksBy.Text = "Album";
        SearchArgID.Text = AlbumDDL.SelectedValue;
        //refresh the track list display
        TracksSelectionList.DataBind();
    }

    protected void PlayListFetch_Click(object sender, EventArgs e)
    {
        //code to go here
        //standard query lookup
        if (string.IsNullOrEmpty(PlaylistName.Text))
        {
            //able to display a message to the user via the MessageUserControl
            //one of the methods of MessageUserControl is .ShowInfo()
            MessageUserControl.ShowInfo("Warning", "Playlist Name is required.");
        }
        else
        {
            //obtain the username from the security Identity class
            string username = User.Identity.Name;

            //the MessageUserControl has embedded in its code the try/catch logic
            //you do not need to code your own try/catch
            MessageUserControl.TryRun(() =>
            {
                //code to be run under the "watchful eyes" of the user control
                //this is the try{your code} of the try/catch
                PlaylistTracksController sysmgr = new PlaylistTracksController();
                List<UserPlaylistTrack> info = sysmgr.List_TracksForPlaylist(PlaylistName.Text, username);
                PlayList.DataSource = info;
                PlayList.DataBind();
            },"","Here is your current playlist.");
        }
    }

    protected void TracksSelectionList_ItemCommand(object sender, 
        ListViewCommandEventArgs e)
    {
        //code to go here
        if (string.IsNullOrEmpty(PlaylistName.Text))
        {
            //able to display a message to the user via the MessageUserControl
            //one of the methods of MessageUserControl is .ShowInfo()
            MessageUserControl.ShowInfo("Warning", "Playlist Name is required.");
        }
        else
        {
            string username = User.Identity.Name;

            //where does TrackId come from
            // ListViewCommandEcventargs e contains the parameter values for this event ; CommandArgument
            //CommandArgument is an object

            int trackid = int.Parse(e.CommandArgument.ToString());

            //send your collection of parameter values to the BLL for processing

            MessageUserControl.TryRun(() =>
            {
                //the process
                PlaylistTracksController sysmgr = new PlaylistTracksController();
                List<UserPlaylistTrack> refreshResults = sysmgr.Add_TrackToPLaylist(PlaylistName.Text, username, trackid);
                PlayList.DataSource = refreshResults;
                PlayList.DataBind();
        }, "Success", "your track has been added to your playlist");
        }
    }

    protected void MoveUp_Click(object sender, EventArgs e)
    {
        //code to go here

        //is there a list

        if (PlayList.Rows.Count == 0)
        {
            MessageUserControl.ShowInfo("Warning", "No play list has been retreived.");
        }
        else
        {
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Warning", "No play list name has been entered.");
            }
            else
            {
                //check only one row has been selected

                int trackid = 0;
                int tracknumber = 0;
                int rowsSelected = 0; // counter

                CheckBox playlistselection = null;

                //traverse the gridview checking each row for a checked box

                for (int i= 0; 1< PlayList.Rows.Count; i++)
                {
                    //playlistselection will point to the current checkbox of the current gridview row being examined

                    playlistselection = PlayList.Rows[i].FindControl("Selected") as CheckBox;

                    if (playlistselection.Checked)
                    {
                        trackid = int.Parse((PlayList.Rows[i].FindControl("TrackId") as Label).Text);
                        tracknumber = int.Parse((PlayList.Rows[i].FindControl("TrackNumber") as Label).Text);
                        rowsSelected++;
                    }
                }//eo for

                //how many tracks were selected

                if(rowsSelected != 1)
                {
                    MessageUserControl.ShowInfo("Warning", "Select only one track");
                }
                else
                {
                    //is this the top track

                    if(tracknumber == 1)
                    {
                        MessageUserControl.ShowInfo("Information", "Track cannot be move up");
                    }
                    else
                    {
                        MoveTrack(trackid, tracknumber, "up");
                    }
                }
            }
        }
    }

    protected void MoveDown_Click(object sender, EventArgs e)
    {
        //code to go here

        if (PlayList.Rows.Count == 0)
        {
            MessageUserControl.ShowInfo("Warning", "No play list has been retreived.");
        }
        else
        {
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Warning", "No play list name has been entered.");
            }
            else
            {
                //check only one row has been selected

                int trackid = 0;
                int tracknumber = 0;
                int rowsSelected = 0; // counter

                CheckBox playlistselection = null;

                //traverse the gridview checking each row for a checked box

                for (int i = 0; 1 < PlayList.Rows.Count; i++)
                {
                    //playlistselection will point to the current checkbox of the current gridview row being examined

                    playlistselection = PlayList.Rows[i].FindControl("Selected") as CheckBox;

                    if (playlistselection.Checked)
                    {
                        trackid = int.Parse((PlayList.Rows[i].FindControl("TrackId") as Label).Text);
                        tracknumber = int.Parse((PlayList.Rows[i].FindControl("TrackNumber") as Label).Text);
                        rowsSelected++;
                    }
                }//eo for

                //how many tracks were selected

                if (rowsSelected != 1)
                {
                    MessageUserControl.ShowInfo("Warning", "Select only one track");
                }
                else
                {
                    //is this the top track

                    if (tracknumber == PlayList.Rows.Count)
                    {
                        MessageUserControl.ShowInfo("Information", "Track cannot be move up");
                    }
                    else
                    {
                        MoveTrack(trackid, tracknumber, "down");
                    }
                }
            }
        }
    }
    protected void MoveTrack(int trackid, int tracknumber, string direction)
    {
        //code to go here
        //wrap uo your work under MessageUserControl

        MessageUserControl.TryRun(() =>
        {
            //standard update call to you BLL

            PlaylistTracksController sysmgr = new PlaylistTracksController();

            //call the appropriate BLL method (update)

            sysmgr.MoveTrack(User.Identity.Name, PlaylistName.Text, trackid, tracknumber, direction);

            //refresh the display
            List<UserPlaylistTrack> results = sysmgr.List_TracksForPlaylist(PlaylistName.Text, User.Identity.Name);
            PlayList.DataSource = results;
            PlayList.DataBind();


        },"Success", "Track has been moved" );


       




    }
    protected void DeleteTrack_Click(object sender, EventArgs e)
    {
        if(PlayList.Rows.Count == 0)
        {
            MessageUserControl.ShowInfo("Information", "you need to retrive a play list before deleting");
        }
        else
        {
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Information", "you need to supply the play list name");
            }
            else
            {
                //colection the tracks to delete
                List<int> trackstodelete = new List<int>();
                int selectedrows = 0;
                CheckBox theBox = null;

                for (int i = 0; i < PlayList.Rows.Count; i++)
                {
                    theBox = PlayList.Rows[i].FindControl("Selected") as CheckBox;

                    if(theBox.Checked)
                    {
                        //Label agvrow = PlayList.Rows[i].FindControl("Selected") as Label;
                        //int trackid = int.Parse(agvrow.Text);
                        //trackstodelete.Add(trackid);


                        trackstodelete.Add(int.Parse((PlayList.Rows[i].FindControl("Selected") as Label).Text));

                        selectedrows++;

                    }
                    if(selectedrows == 0)
                    {
                        MessageUserControl.ShowInfo("Information", "you need to select one or more tracks to remove");
                    }
                    else
                    {
                        //at this point you have your data for BLL processing
                        MessageUserControl.TryRun(() =>
                        {
                            PlaylistTracksController sysmgr = new PlaylistTracksController();

                            sysmgr.DeleteTracks(User.Identity.Name, PlaylistName.Text, trackstodelete);

                            List<UserPlaylistTrack> info = sysmgr.List_TracksForPlaylist(PlaylistName.Text, User.Identity.Name);

                            PlayList.DataSource = info;
                            PlayList.DataBind();

                        },"Success", "Tracks Removed");
                    }
                }
            }

        }
    }
}
