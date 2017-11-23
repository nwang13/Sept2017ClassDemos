using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using Chinook.Data.Entities;
using ChinookSystem.DAL;
using System.ComponentModel;  //expose methods for ODS wizard
using Chinook.Data.POCOs;
#endregion

namespace ChinookSystem.BLL
{
    [DataObject]
    public class AlbumController
    {
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<ArtistAlbumByReleaseYear> Albums_ListforArtist(int artistid)
        {
            using (var context = new ChinookContext())
            {
                var results = from x in context.Albums
                              where x.ArtistId.Equals(artistid)
                              select new ArtistAlbumByReleaseYear
                              {
                                  Title = x.Title,
                                  Released = x.ReleaseYear
                              };
                return results.ToList();
            }
        }//eom

        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<Album> Albums_ListByYearRelease(int minyear, int maxyear)
        {
            using (var context = new ChinookContext())
            {
                var results = from x in context.Albums
                              where x.ReleaseYear >= minyear && x.ReleaseYear <= maxyear
                              orderby x.ReleaseYear, x.Title
                              select x;
                return results.ToList();
            }
        }//eom


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Album> Albums_ListByTitle(string title)
        {
            using (var context = new ChinookContext())
            {
                var results = from x in context.Albums
                              where x.Title.Contains(title)
                              orderby x.Title, x.ReleaseYear
                              select x;
                return results.ToList();
            }
        }//eom

        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<Album> Albums_List()
        {
            using (var context = new ChinookContext())
            {
                return context.Albums.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public Album Albums_Get(int albumid)
        {
            using (var context = new ChinookContext())
            {
                return context.Albums.Find(albumid);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Insert,false)]
        public int Albums_Add(Album item)
        {
            using (var context = new ChinookContext())
            {
                item = context.Albums.Add(item);    //staging
                context.SaveChanges();              //commit of the request
                return item.AlbumId;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update,false)]
        public int Albums_Update(Album item)
        {
            using (var context = new ChinookContext())
            {
                context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                return context.SaveChanges();
            }
        }

        public int Albums_Delete(int albumid)
        {
            using (var context = new ChinookContext())
            {
                var existingItem = context.Albums.Find(albumid);
                context.Albums.Remove(existingItem);
                return context.SaveChanges();

            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete,false)]
        public int Albums_Delete(Album item)
        {
           return Albums_Delete(item.AlbumId);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SelectionList> List_AlbumTitles()
        {
            using (var context = new ChinookContext())
            {
                var results = from x in context.Albums
                              orderby x.Title
                              select new SelectionList
                              {
                                  IDValueField = x.AlbumId,
                                  DisplayText = x.Title
                              };
                return results.ToList();
            }
        }
    }
}
