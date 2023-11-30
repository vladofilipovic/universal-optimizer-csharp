using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphFileTransformations
{
    public class GranaGrafa : IComparable<GranaGrafa>
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
            if (izvorId - other.izvorId != 0)
                return (izvorId - other.izvorId);
            if (odredisteId - other.odredisteId != 0)
                return (odredisteId - other.odredisteId);
            if (tezina - other.tezina != 0)
                return (int)(tezina - other.tezina);
            return oznaka.CompareTo(other.oznaka);
        }
    }

}
