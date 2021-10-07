using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using HtmlAgilityPack;
using PkmnOrm.Models;

namespace PkmnScraper
{
    class Program
    {
        static List<PkmnOrm.Models.EvolveTo> evolutionList = new();

        // source: https://pokemondb.net/pokedex/national
        static string href = @"https://pokemondb.net/pokedex/national";
        static List<HtmlNode> htmlNodes = new();
        static HtmlDocument htmlDoc = new();
        static HashSet<string> pkmnUrls = new();
        static string htmlText = "";
        static readonly int tenSeconds = 10000;
        static List<Pokemon> pokemonList = new();
        static Types types = new();
        static HashSet<string> pkmnImageUrls = new();
        static List<string> pkmnImageUrlsClean = new();
        static int pkmnId = 0;

        static void Main(string[] args)
        {
            htmlText = GetHtmlAsString(href);
            htmlDoc.LoadHtml(htmlText);
            GetNodesFromHtmlDoc("//div[@class[contains(., 'infocard')]]");
            ExtractUrlsFromNodes();

            //foreach (string url in pkmnUrls)
            //{
            //    Thread.Sleep(tenSeconds);
            //    CreateListOfPokemon(url);
            //}
            //PkmnOrm.Database.ToDotJson.Write(pokemonList);

            ExtractImgUrlsFromNodes();
            CleanUpUrls();
            DownloadPokemonImages();
        }

        private static void CleanUpUrls()
        {
            foreach(string url in pkmnImageUrls)
            {
                pkmnImageUrlsClean.Add(
                    url
                    .Replace(": ", "-")     //Type: Null
                    .Replace("♀", "-f")     //Nidoran (female)
                    .Replace("♂", "-m")     //Nidoran (male)
                    .Replace("'", "")       //Farfetch'd
                    .Replace(". ", "-")     //Mr. Mime
                    .Replace(" ", "-")      //Several Gen7 PKMN with two names
                    .Replace("jr.", "jr")   //Mime Jr.
                    .Replace("giratina", "giratina-origin")     // Seriously?
                    .Replace("shaymin", "shaymin-land")         // ...
                    .Replace("é", "e").Replace("è", "e")        // Flabébé etc.
                    .Replace("oricorio", "oricorio-baile")      // It has four variants.
                    .Replace("lycanroc", "lycanroc-midday")
                    .Replace("wishiwashi", "wishiwashi-solo")
                    .Replace("eiscue", "eiscue-ice")
                    .Replace("morpeko", "morpeko-full-belly")
                    .Replace("urshifu", "urshifu-single-strike")
                    );
            }
        }

        private static void ExtractImgUrlsFromNodes()
        {
            foreach(HtmlNode node in htmlNodes)
            {
                pkmnImageUrls.Add(@$"https://img.pokemondb.net/artwork/large/{
                    node.SelectSingleNode(".//a[@class[contains(., 'ent-name')]]").InnerText.ToLower()
                    }.jpg");
            }
        }

        private static void DownloadPokemonImages()
        {
            for (; pkmnId < 898;) // for-loop för att kunna starta om utan att behöva ladda ner alla som fungerade igen.
            {
                pkmnId++;
                Thread.Sleep(tenSeconds);
                using WebClient client = new();
                client.DownloadFile(pkmnImageUrlsClean[pkmnId-1], $@"C:\plushogskolan\Webbutveckling Backend\PkmnWebApi\PkmnOrm\Images\{pkmnId}.jpg");
            }
        }

        private static void CreateListOfPokemon(string url)
        {
            string pkmnHtml = GetHtmlAsString(url);
            HtmlDocument pkmnDoc = new();
            pkmnDoc.LoadHtml(pkmnHtml);
            Pokemon pkmn = new();
            List<HtmlNode> nodeList = GetPokemonNodeList(pkmnDoc);
            pkmn.Id = Convert.ToInt32(nodeList[0].InnerText);
            pkmn.Name = nodeList[1].InnerText;
            pkmn.Type1 = types.TypeToInt[nodeList[2].InnerText];
            if (nodeList.Count > 3) pkmn.Type2 = types.TypeToInt[nodeList[3].InnerText];
            pokemonList.Add(pkmn);
        }

        private static List<HtmlNode> GetPokemonNodeList(HtmlDocument pkmnDoc)
        {
            var nodeList = new List<HtmlNode>();
            nodeList.Add(pkmnDoc.DocumentNode.SelectSingleNode("//table[@class[contains(., 'vitals-table')]]/tbody/tr/td/strong"));
            nodeList.Add(pkmnDoc.DocumentNode.SelectSingleNode("//main/h1"));
            var typeNodes = pkmnDoc.DocumentNode.SelectNodes("//table[@class[contains(., 'vitals-table')]]/tbody/tr/td/a[@class[contains(., 'type')]]");
            nodeList.Add(typeNodes[0]);
            if (typeNodes.Count > 1)
            {
                nodeList.Add(typeNodes[1]);
            }
            return nodeList;
        }

        private static void ExtractUrlsFromNodes()
        {
            foreach (HtmlNode node in htmlNodes)
            {
                pkmnUrls.Add(@$"https://pokemondb.net{
                    node.SelectSingleNode(".//span[2]/a[1]")
                    .GetAttributeValue<string>("href", @"\error")
                    }");
            }
        }

        private static string GetHtmlAsString(string url)
        {
            using WebClient client = new WebClient();
            return client.DownloadString(url);
        }

        private static void GetNodesFromHtmlDoc(string xpath)
        {
            foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes(xpath))
            {
                htmlNodes.Add(node);
            }
        }
    }
}
