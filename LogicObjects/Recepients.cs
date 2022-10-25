using System.Collections;

namespace LogicObjects
{
    public class Recepients : IEnumerable<Recepient>
    {
        private ICollection<Recepient> collRecepient;
        public Recepients()
        {
            collRecepient = new HashSet<Recepient>();
        }

        public Recepient AddRecepient(Recepient rp)
        {
            collRecepient.Add(rp);

            return rp;
        }

        public bool RecepientExists(string RecepientID)
        {
            Recepient rp = collRecepient.FirstOrDefault(rp => rp.RecepientId == RecepientID);

            if (rp == null)
            {
                return false;
            }

            return true;
        }

        public Recepient GetRecepient(string RecepientID)
        {
            Recepient rp = collRecepient.FirstOrDefault(rp => rp.RecepientId == RecepientID);

            return rp;
        }

        public IEnumerator<Recepient> GetEnumerator()
        {
            return collRecepient.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return collRecepient.GetEnumerator();
        }

        ~Recepients()
        {

        }
    }
}