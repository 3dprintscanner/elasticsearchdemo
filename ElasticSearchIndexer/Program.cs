using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using Newtonsoft.Json;

namespace ElasticSearchIndexer
{
    class Program
    {
        static void Main(string[] args)
        {

            var corpusFile = System.IO.File.ReadAllText("CORPUSExtract.json");

            var tipLocData = JsonConvert.DeserializeObject<RootObject>(corpusFile);


            var node = new Uri("http://localhost:9200");

            var settings = new ConnectionSettings(node
                );

            var client = new ElasticClient(settings);

            client.DeleteIndex("stationsv5");

            var index = new CreateIndexRequest("stationsv5");

            var mapping = new MappingsDescriptor();
           MappingsDescriptor mappingsDescriptor = mapping.Map<TIPLOCDATA>(descriptor => descriptor.AutoMap());
           TypeMappingDescriptor<TIPLOCDATA> typeMappingDescriptor = new TypeMappingDescriptor<TIPLOCDATA>();
           typeMappingDescriptor.AutoMap();


            ITypeMapping mappingtype = new TypeMappingDescriptor<TIPLOCDATA>();
            
            
            var ix = new IndexSettings();

            
            
            var ad = new AnalysisDescriptor();
            ad.Tokenizers(tk => tk.NGram("my_tokeniser", my => my.MinGram(2).MaxGram(5)));
            ad.Analyzers(a => a.Custom("my_custom_analyser", cus => cus.Tokenizer("my_tokeniser").Filters("lowercase")));
            ix.Analysis = new AnalysisDescriptor();


            ix.Analysis = ad;

            var type = TypeName.Create(typeof (TIPLOCDATA));
            
            index.Settings = ix;
            index.Mappings = new Mappings{{type,typeMappingDescriptor}};
            client.CreateIndex(index);



            var bulkRequest = new BulkRequest("stationsv5");
            bulkRequest.Operations = new List<IBulkOperation>();
            var tiploclist = tipLocData.TIPLOCDATA;

            foreach (var tiploc in tiploclist)
            {
                if (tiploc.CRS != null && tiploc.CRS.Length == 3)
                {
                    IBulkOperation addOperation = new BulkIndexOperation<TIPLOCDATA>(tiploc);
                    bulkRequest.Operations.Add(addOperation);
                }

            }

            client.Bulk(bulkRequest);


        }
    }
}
[ElasticsearchType]
public class TIPLOCDATA
{
    public string CRS { get; set; }
    public string NLC { get; set; }
    [String(Analyzer = "my_custom_analyser")]
    public string NLCDESC { get; set; }
    [String(Analyzer = "my_custom_analyser")]
    public string NLCDESC16 { get; set; }
    public string STANOX { get; set; }
    public string TIPLOC { get; set; }
    public string UIC { get; set; }
}

public class RootObject
{
    public List<TIPLOCDATA> TIPLOCDATA { get; set; }
}
