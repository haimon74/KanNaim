﻿using System;
using System.Linq;
using System.Linq.Expressions;


namespace Kan_Naim_Main.DataAccess
{
    public class Lookup
    {
        public static readonly DataClassesKanNaimDataContext Db = new DataClassesKanNaimDataContext();

        public static IQueryable<Table_LookupPhotoType> GetLookupPhotoTypes()
        {
            return from c in Db.Table_LookupPhotoTypes
                   select c;
        }

        public static int GetLookupCategoryIdFromName(string name)
        {
            try
            {
                var result = (from c in Db.Table_LookupCategories
                              where c.CatEnglishName == name || c.CatHebrewName == name
                              select c).Single().CatId;
                return result;
            }
            catch (Exception)
            {
                return -1;
            }
            
        }

        public static int GetLookupReporterIdFromName(string name)
        {
            try
            {
                var result = (from c in Db.Table_LookupReporters
                              where c.PublishNameLong == name || c.PublishNameShort == name
                              select c).Single().UserId;
                return result;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public static string GetLookupCategoryNameFromId(int id)
        {
            try
            {
                var result = (from c in Db.Table_LookupCategories
                              where c.CatId == id
                              select c).Single().CatHebrewName;
                return result;
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static Table_LookupReporter GetLookupReporterFromUserId(int id)
        {
            return (from c in Db.Table_LookupReporters
                    where c.UserId == id
                    select c).Single();
        }
        public static int GetLookupIndexCategoryNumberFromCategoryName(string name)
        {
            var  result = from c in Db.Table_LookupIndexes
                          where c.IndexName.Trim() == name.Trim()
                          select c.Id;
            return result.First();
        }
    }
}
