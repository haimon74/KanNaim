

namespace DbSql
{
    public class Delete
    {
        private static readonly DataClassesKanNaimDataContext Db = new DataClassesKanNaimDataContext();

        public static void TableArticle(Table_Article article)
        {
            //TODO transaction

            int articleId = article.ArticleId;

            var delTaks = Filter.GetTaktzirimByArticleId(articleId);

            foreach (var tak in delTaks)
            {
                TableTaktzirim(tak);
            }

            Db.Table_Articles.DeleteOnSubmit(article);
            Db.SubmitChanges();
        }
        public static void TableArticle(int id)
        {
            TableArticle(Filter.GetArticleByArticleId(id));
        }

        public static void TableTaktzirim(Table_Taktzirim tak)
        {
            Db.Table_Taktzirims.DeleteOnSubmit(tak);
            Db.SubmitChanges();
        }

        public static void TableOriginalImages(Table_OriginalPhotosArchive originalPhoto)
        {
            // TODO transaction

            int photoId = originalPhoto.PhotoId;

            var delPhotos = Filter.GetPhotosArchiveByOriginalPhotoId(photoId);

            foreach (var photo in delPhotos)
            {
                Db.Table_PhotosArchives.DeleteOnSubmit(photo);
            }

            Db.Table_OriginalPhotosArchives.DeleteOnSubmit(originalPhoto);
            Db.SubmitChanges();
        }
        public static void TableOriginalImages(int id)
        {
            TableOriginalImages(Filter.GetOriginalPhotoFromId(id));
        }
    }
}
