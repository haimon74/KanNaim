using System.Linq;

namespace Kan_Naim_Main.DataAccess
{
    class Insert
    {
        private static readonly DataClassesKanNaimDataContext Db = new DataClassesKanNaimDataContext();


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
    }
}
