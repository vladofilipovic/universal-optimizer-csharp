using System;
using System.IO;

namespace GraphFileTransformations
{
    class FromPlainToGraphml
    {
        public static int transform(string putanjaUlaz, string putanjaIzlaz)
        {
            string linija;
            int brojCvorova, brojGrana;
            int[] poc;
            int[] kraj;

            try
            {
                StreamReader citac = new(putanjaUlaz);
                linija = citac.ReadLine()!.Trim();
                string[] parcici = linija.Split(' ');
                brojCvorova = Convert.ToInt32(parcici[0]);
                brojGrana = Convert.ToInt32(parcici[1]);
                poc = new int[brojGrana];
                kraj = new int[brojGrana];
                for (int i = 0; i < brojGrana; i++)
                {
                    linija = citac.ReadLine()!.Trim();
                    string[] delovi = linija.Split(' ');
                    poc[i] = Convert.ToInt32(delovi[0]);
                    kraj[i] = Convert.ToInt32(delovi[1]);
                }
                citac.Close();
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
                return -11;
            }

            string prolog =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine +
                "<graphml xmlns=\"http://graphml.graphdrawing.org/xmlns\">" + Environment.NewLine +
                "<key attr.name = \"label\" attr.type = \"string\" for= \"node\" id = \"label\" />" + Environment.NewLine +
                "<key attr.name = \"Edge Label\" attr.type = \"string\" for= \"edge\" id = \"edgelabel\" />" + Environment.NewLine +
                "<key attr.name = \"weight\" attr.type = \"double\" for= \"edge\" id = \"weight\" />" + Environment.NewLine +
                "<key attr.name = \"Edge Id\" attr.type = \"string\" for= \"edge\" id = \"edgeid\" />" + Environment.NewLine +
                "<key attr.name = \"r\" attr.type = \"int\" for= \"node\" id = \"r\" />" + Environment.NewLine +
                "<key attr.name = \"g\" attr.type = \"int\" for= \"node\" id = \"g\" />" + Environment.NewLine +
                "<key attr.name = \"b\" attr.type = \"int\" for= \"node\" id = \"b\" />" + Environment.NewLine +
                "<key attr.name = \"x\" attr.type = \"float\" for= \"node\" id = \"x\" />" + Environment.NewLine +
                "<key attr.name = \"y\" attr.type = \"float\" for= \"node\" id = \"y\" />" + Environment.NewLine +
                "<key attr.name = \"size\" attr.type = \"float\" for= \"node\" id = \"size\" />" + Environment.NewLine +
                "<graph edgedefault = \"undirected\" > " + Environment.NewLine;
            string epilog =
                "</graph>" + Environment.NewLine +
                "</graphml>";
            try
            {
                StreamWriter pisac = new(putanjaIzlaz);
                pisac.WriteLine(prolog);
                for (int i = 0; i < brojCvorova; i++)
                {
                    pisac.WriteLine("<node id=\"" + (i + 1) + "\" > ");
                    pisac.WriteLine("<data key = \"label\" >" + (i + 1) + " </data >");
                    pisac.WriteLine("<data key = \"size\" > 3.4 </data >");
                    pisac.WriteLine("<data key = \"r\" > 184 </data >");
                    pisac.WriteLine("<data key = \"g\" > 109 </data >");
                    pisac.WriteLine("<data key = \"b\" > 156 </data >");
                    double d = 3.4 + (i + 1) * 3.6;
                    pisac.WriteLine("<data key = \"x\" >" + d + "</data >");
                    pisac.WriteLine("<data key = \"y\" >" + d + "</data >");
                    pisac.WriteLine("</node >");
                }
                for (int i = 0; i < brojGrana; i++)
                {
                    pisac.WriteLine("<edge source = \"" + poc[i] + "\" target = \"" + kraj[i] + "\" >");
                    pisac.WriteLine("<data key = \"weight\" > 1.0 </data >");
                    pisac.WriteLine("</edge >");
                }
                pisac.WriteLine(epilog);
                pisac.Flush();
                pisac.Close();
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
                return -12;
            }
            return 0;
        }
    }
}
