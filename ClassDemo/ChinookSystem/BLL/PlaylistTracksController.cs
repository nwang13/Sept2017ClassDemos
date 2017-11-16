using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using Chinook.Data.Entities;
using Chinook.Data.DTOs;
using Chinook.Data.POCOs;
using ChinookSystem.DAL;
using System.ComponentModel;
#endregion

namespace ChinookSystem.BLL
{
    public class PlaylistTracksController
    {
        public List<UserPlaylistTrack> List_TracksForPlaylist(
            string playlistname, string username)
        {
            using (var context = new ChinookContext())
            {

                //code to go here
                //what would happen if there is no match for the incoming parameter values
                //we need to ensure that the results have a valid value
                //this value will the resolve of a query either a null(not found_)
                //or an IEnumerable<T> collection
                //to achieve a valid value encapulate the query in a 
                // .FirstOrDefault()

                var results = (from x in context.Playlists
                               where x.UserName.Equals(username)
                               && x.Name.Equals(playlistname)
                               select x).FirstOrDefault();
                //after: test if you should return a null as your collection
                //or find the tracks for the given PlaylistId in results
                if(results == null)
                {
                    return null;
                }
                else
                {

                    var theTracks = from x in context.PlaylistTracks
                                    where x.PlaylistId.Equals(results.PlaylistId)
                                    orderby x.TrackNumber
                                    select new UserPlaylistTrack
                                    {
                                        TrackID = x.TrackId,
                                        TrackNumber = x.TrackNumber,
                                        TrackName = x.Track.Name,
                                        Milliseconds = x.Track.Milliseconds,
                                        UnitPrice = x.Track.UnitPrice
                                    };


                    return theTracks.ToList();
                }
                //now get the tracks


            }
        }//eom
        public List<UserPlaylistTrack> Add_TrackToPLaylist(string playlistname, string username, int trackid)
        {
            using (var context = new ChinookContext())
            {
                //code to go here
                //part one
                //query to ger the playlistID
                var exists = (from x in context.Playlists
                               where x.UserName.Equals(username)
                               && x.Name.Equals(playlistname)
                               select x).FirstOrDefault();
                //initialize the tracknumber

                int tracknumber = 0;

                //will need to create an instance of playlistTrack

                PlaylistTrack newTrack = null;

                //determine if a playlist "parent" instances needs to be created

                if (exists == null)
                {
                    //this is a new playlist
                    //create an instance of playlist to add to playlist table
                    exists = new Playlist();

                    exists.Name = playlistname;
                    exists.UserName = username;
                    exists = context.Playlists.Add(exists);

                    //at this time there is NO physical pkey
                    //the psuedo pkey is handled by the HashSet

                    tracknumber = 1;  
                }
                else
                {
                    //playlist exists
                    //need to generate the next track number

                    tracknumber = exists.PlaylistTracks.Count() + 1;

                    //validation: in our example a track can ONLY exist once
                    // on a particular playlist

                    newTrack = exists.PlaylistTracks.SingleOrDefault(x => x.TrackId == trackid);

                    if(newTrack != null)
                    {
                        throw new Exception("PlayList already has requested track");
                    }                    
                }
                //part two: Add tje playlisttrack instance
                //use navigation to .Add the nre track to Playlisttrack

                newTrack = new PlaylistTrack();
                newTrack.TrackId = trackid;
                newTrack.TrackNumber = tracknumber;

                //note: the pkey for playlistId may not yet exist
                //using navigation one can let hashSet handle the playlistId pkey value

                exists.PlaylistTracks.Add(newTrack);

                //physically add all data to the database
                //commit

                context.SaveChanges();

                return List_TracksForPlaylist(playlistname, username);

            }
        }//eom
        public void MoveTrack(string username, string playlistname, int trackid, int tracknumber, string direction)
        {
            using (var context = new ChinookContext())
            {
                //code to go here 

                var exists = (from x in context.Playlists
                              where x.UserName.Equals(username)
                              && x.Name.Equals(playlistname)
                              select x).FirstOrDefault();
                if(exists == null)
                {
                    throw new Exception("Play List has been removed from the file");
                }
                else
                {
                    PlaylistTrack moveTrack = (from x in exists.PlaylistTracks
                                               where x.TrackId == trackid
                                               select x).FirstOrDefault();

                    if (moveTrack == null)
                    {
                        throw new Exception("Play List track has been removed from the file");
                    }
                    else
                    {
                        PlaylistTrack otherTrack = null;

                        if(direction.Equals("up"))
                        {
                            //up
                            if(moveTrack.TrackNumber == 1)
                            {
                                throw new Exception("Play List track already at top");
                            }
                            else
                            {
                                otherTrack = (from x in exists.PlaylistTracks
                                              where x.TrackNumber == moveTrack.TrackNumber - 1
                                              select x).FirstOrDefault();
                                if(otherTrack == null)
                                {
                                    throw new Exception("Other play List track is missing");
                                }
                                else
                                {
                                    moveTrack.TrackNumber -= 1;
                                    otherTrack.TrackNumber += 1; 
                                }
                            }
                        }
                        else
                        {
                            //down

                            if (moveTrack.TrackNumber == exists.PlaylistTracks.Count)
                            {
                                throw new Exception("Play List track already at bottom");
                            }
                            else
                            {
                                otherTrack = (from x in exists.PlaylistTracks
                                              where x.TrackNumber == moveTrack.TrackNumber + 1
                                              select x).FirstOrDefault();
                                if (otherTrack == null)
                                {
                                    throw new Exception("Other play List track is missing");
                                }
                                else
                                {
                                    moveTrack.TrackNumber += 1;
                                    otherTrack.TrackNumber -= 1;
                                }
                            }
                        }//eof up/down

                        //staging

                        context.Entry(moveTrack).Property(y => y.TrackNumber).IsModified = true;
                        context.Entry(otherTrack).Property(y => y.TrackNumber).IsModified = true;

                        //saving

                        context.SaveChanges();
                    }
                }

            }
        }//eom


        public void DeleteTracks(string username, string playlistname, List<int> trackstodelete)
        {
            using (var context = new ChinookContext())
            {
               //code to go here


            }
        }//eom
    }
}
