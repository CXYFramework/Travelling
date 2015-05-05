using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Version = Lucene.Net.Util.Version;
using System.IO;
using Travelling.Model.DTO;
namespace Travelling.LuceneSearch
{
    public static class Lucene
    {

        private static string _luceneDir =
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lucene_index");
        private static FSDirectory _directoryTemp;
        private static FSDirectory _directory
        {
            get
            {
                if (_directoryTemp == null)
                    _directoryTemp = FSDirectory.Open(new DirectoryInfo(_luceneDir));

                if (IndexWriter.IsLocked(_directoryTemp))
                    IndexWriter.Unlock(_directoryTemp);

                var lockFilePath = Path.Combine(_luceneDir, "write.lock");
                if (File.Exists(lockFilePath))
                    File.Delete(lockFilePath);

                return _directoryTemp;
            }
        }

        private static void _addToLuceneIndex(HotelDescriptionDTO hotelDescriptionDTO, IndexWriter writer)
        {
            // remove older index entry
            var searchQuery = new TermQuery(new Term("Id", hotelDescriptionDTO.Id.ToString()));
            writer.DeleteDocuments(searchQuery);

            // add new index entry
            var doc = new Document();

            // add lucene fields mapped to db fields
            doc.Add(new Field("Id", hotelDescriptionDTO.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("HotelName", hotelDescriptionDTO.HotelName, Field.Store.YES, Field.Index.ANALYZED));
           
            doc.Add(new Field("HotelCityCode", hotelDescriptionDTO.HotelCityCode.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("AreaID", hotelDescriptionDTO.AreaID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

            doc.Add(new Field("AddressLine", hotelDescriptionDTO.AddressLine, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Description", hotelDescriptionDTO.Description, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("DescriptionText", hotelDescriptionDTO.DescriptionText, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Url", hotelDescriptionDTO.Url, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("MinPrice", hotelDescriptionDTO.MinPrice.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            // add entry to index
            writer.AddDocument(doc);
        }

        public static void AddUpdateLuceneIndex(IEnumerable<HotelDescriptionDTO> hotelDescriptionDTOs)
        {
            // init lucene
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // add data to lucene search index (replaces older entry if any)
                foreach (var hotelDescriptionDTO in hotelDescriptionDTOs)
                    _addToLuceneIndex(hotelDescriptionDTO, writer);

                // close handles
                analyzer.Close();
                writer.Dispose();
            }
        }

        public static void AddUpdateLuceneIndex(HotelDescriptionDTO hotelDescriptionDTO)
        {
             AddUpdateLuceneIndex(new List<HotelDescriptionDTO> { hotelDescriptionDTO });
        }

        public static void ClearLuceneIndexRecord(int record_id)
        {
            // init lucene
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // remove older index entry
                var searchQuery = new TermQuery(new Term("Id", record_id.ToString()));

                writer.DeleteDocuments(searchQuery);

                // close handles
                analyzer.Close();
                writer.Dispose();
            }
        }

        public static bool ClearLuceneIndex()
        {
            try
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                using (var writer = new IndexWriter(_directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    // remove older index entries
                    writer.DeleteAll();

                    // close handles
                    analyzer.Close();
                    writer.Dispose();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static void Optimize()
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                analyzer.Close();
                writer.Optimize();
                writer.Dispose();
            }
        }

        private static HotelDescriptionDTO _mapLuceneDocumentToData(Document doc)
        {
            return new HotelDescriptionDTO
            {
                Id = doc.Get("Id"),
                HotelName = doc.Get("HotelName"),
                HotelCityCode = Convert.ToInt32(doc.Get("HotelCityCode")),
                AreaID = Convert.ToInt32(doc.Get("AreaID")),
                AddressLine = doc.Get("AddressLine"),
                DescriptionText = doc.Get("DescriptionText"),
                Url = doc.Get("Url"),
                MinPrice = Convert.ToDecimal(doc.Get("MinPrice")),
                Description = doc.Get("Description")

            };
        }

        private static IEnumerable<HotelDescriptionDTO> _mapLuceneToDataList(IEnumerable<Document> hits)
        {
            return hits.Select(_mapLuceneDocumentToData).ToList();
        }

        private static IEnumerable<HotelDescriptionDTO> _mapLuceneToDataList(IEnumerable<ScoreDoc> hits,
            IndexSearcher searcher)
        {
            return hits.Select(hit => _mapLuceneDocumentToData(searcher.Doc(hit.Doc))).ToList();
        }

        private static Query parseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }
            return query;
        }

        private static IEnumerable<HotelDescriptionDTO> _search(string searchQuery, string searchField = "")
        {
            // validation
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", "")))
                return new List<HotelDescriptionDTO>();

            // set up lucene searcher
            using (var searcher = new IndexSearcher(_directory, false))
            {
                var hits_limit = 2;
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);

                // search by single field
                //if (!string.IsNullOrEmpty(searchField))
                //{
                var parser = new QueryParser(Version.LUCENE_30, searchField, analyzer);
                //var query = parseQuery(searchQuery, parser);

                var query = new TermQuery(new Term(searchField, searchQuery));

                var hits = searcher.Search(query, hits_limit).ScoreDocs;
                var results = _mapLuceneToDataList(hits, searcher);
                analyzer.Close();
                searcher.Dispose();
                return results;


                //}
                //// search by multiple fields (ordered by RELEVANCE)
                //else
                //{
                //    //var parser = new MultiFieldQueryParser
                //    //    (Version.LUCENE_30, new[] { "Id", "Name", "Description" }, analyzer);
                //    //var query = parseQuery(searchQuery, parser);

                //    //var hits = searcher.Search
                //    //(query, null, hits_limit, Sort.RELEVANCE).ScoreDocs;
                //    //var results = _mapLuceneToDataList(hits, searcher);
                //    //analyzer.Close();
                //    //searcher.Dispose();
                //    //return results;
                //}
            }
        }

        public static IEnumerable<HotelDescriptionDTO> Search(string input, string fieldName = "")
        {
            if (string.IsNullOrEmpty(input)) return new List<HotelDescriptionDTO>();

            //var terms = input.Trim().Replace("-", " ").Split(' ')
            //    .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
            //input = string.Join(" ", terms);

            return _search(input, fieldName);
        }

        public static IEnumerable<HotelDescriptionDTO> SearchDefault(string input, string fieldName = "")
        {
            return string.IsNullOrEmpty(input) ? new List<HotelDescriptionDTO>() : _search(input, fieldName);
        }

        public static IEnumerable<HotelDescriptionDTO> GetAllIndexRecords()
        {
            // validate search index
            if (!System.IO.Directory.EnumerateFiles(_luceneDir).Any())
                return new List<HotelDescriptionDTO>();

            // set up lucene searcher
            var searcher = new IndexSearcher(_directory, false);
            var reader = IndexReader.Open(_directory, false);
            var docs = new List<Document>();
            var term = reader.TermDocs();
            while (term.Next())
                docs.Add(searcher.Doc(term.Doc));

            reader.Dispose();
            searcher.Dispose();
            return _mapLuceneToDataList(docs);
        }
    }
}
