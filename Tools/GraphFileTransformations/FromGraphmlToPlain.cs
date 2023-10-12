using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace GraphFileTransformations
{
    public class CvorGrafa: IComparable<CvorGrafa>
    {
        public int id;
        public string oznaka;

        public CvorGrafa(int id, string oznaka)
        {
            this.id = id;
            this.oznaka = oznaka;
        }

        public int CompareTo(CvorGrafa other)
        {
            if (id - other.id != 0)
                return (id - other.id);
            return oznaka.CompareTo(other.oznaka);
        }
    };

    public class GranaGrafa: IComparable<GranaGrafa>
    {
        public int izvorId;
        public int odredisteId;
        public string oznaka;
        public double tezina;

        public GranaGrafa(int izvorId, int odredisteId, string oznaka, double tezina)
        {
            this.izvorId = izvorId;
            this.odredisteId = odredisteId;
            this.oznaka = oznaka;
            this.tezina = tezina;
        }

        public int CompareTo(GranaGrafa other)
        {
            if( izvorId - other.izvorId != 0)
                return (izvorId - other.izvorId);
            if (odredisteId - other.odredisteId != 0)
                return (odredisteId - other.odredisteId);
            if (tezina - other.tezina != 0)
                return (int)(tezina - other.tezina);
            return oznaka.CompareTo(other.oznaka);
        }
    }

    class FromGraphmlToPlain
    {
        static int pozicija(int[] niz, int vrednost)
        {
            for (int i = 0; i < niz.Length; i++)
                if (niz[i] == vrednost)
                    return (i + 1);
            return -1;
        }

        public static int transform(string putanjaUlaz, string putanjaIzlaz)
        {
            XmlDocument doc = new XmlDocument();
            ISet<CvorGrafa> cvoroviGrafa = new SortedSet<CvorGrafa>();
            int brojCvorova = 0;
            ISet<GranaGrafa> graneGrafa = new SortedSet<GranaGrafa>();
            int brojGrana = 0;
            try
            {
                doc.Load(putanjaUlaz);
                XmlNodeList elementi = doc.DocumentElement.LastChild.ChildNodes;
                foreach (XmlNode elemenat in elementi)
                {
                    if (elemenat.Name == "node")
                    {
                        int id = Convert.ToInt32(elemenat.Attributes["id"].Value);
                        string oznaka = elemenat.FirstChild.InnerText;
                        cvoroviGrafa.Add(new CvorGrafa(id, oznaka));
                    }
                }
                brojCvorova = cvoroviGrafa.Count;

                foreach (XmlNode elemenat in elementi)
                {
                    if (elemenat.Name == "edge")
                    {
                        int sourceId = Convert.ToInt32(elemenat.Attributes["source"].Value);
                        int destinationId = Convert.ToInt32(elemenat.Attributes["target"].Value);
                        string oznaka = elemenat.FirstChild.InnerText;
                        double tezina = Convert.ToDouble(elemenat.LastChild.InnerText);
                        graneGrafa.Add(new GranaGrafa(sourceId, destinationId, oznaka, tezina));
                    }
                }
                brojGrana = graneGrafa.Count;
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
                return -21;
            }

            int[] mapa = new int[brojCvorova];
            int i = 0;
            foreach (CvorGrafa c in cvoroviGrafa)
                mapa[i++] = c.id;
            try
            {
                using (StreamWriter tokZaUpis = new StreamWriter(putanjaIzlaz))
                {
                    tokZaUpis.WriteLine(" " + brojCvorova + "\t" + brojGrana);
                    foreach (GranaGrafa g in graneGrafa)
                    {
                        tokZaUpis.WriteLine(pozicija(mapa, g.izvorId) + "\t"
                            + pozicija(mapa, g.odredisteId) + "\t"
                            + Convert.ToDouble(g.oznaka)
                            );
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
                return -22;
            }
            return 0;
        }
    }
}
