using System.Linq;

namespace DbSql
{
    public class Insert
    {
        public static readonly DataClassesKanNaimDataContext Db = new DataClassesKanNaimDataContext();

        public static int TableArticles(Table_Article tblArticle)
        {
            if (tblArticle.ArticleId <= 0)
                Db.Table_Articles.InsertOnSubmit(tblArticle);

            Db.SubmitChanges();

            Table_Article article = (from c in Db.Table_Articles
                                     where (c.Title == tblArticle.Title)
                                     select c).SingleOrDefault();

            return (article == null) ? -1 : article.ArticleId;
        }
        public static Table_Taktzirim TableTaktzirim(Table_Taktzirim tableTaktzirim)
        {
            Db.Table_Taktzirims.InsertOnSubmit(tableTaktzirim);
            Db.SubmitChanges();
            var taktzirim = Filter.GetTaktzirimByArticleId(tableTaktzirim.ArticleId);
            
            if (taktzirim == null)
                return null;

            return (from c in taktzirim
                    where c.TakTypeId == tableTaktzirim.TakTypeId
                    select c).SingleOrDefault();

        }

        public static Table_Broadcast TableBroadcast(Table_Broadcast broadcast)
        {
            Db.Table_Broadcasts.InsertOnSubmit(broadcast);
            
            Db.SubmitChanges();
            
            return Filter.GetBroadcastByTakIdCatId(broadcast.TakId, broadcast.CategoryId);
        }
    }
}
