using System.Collections;

namespace LogicObjects
{
    public class L_Recepients : IEnumerable<L_Recepient>
    {
        private ICollection<L_Recepient> collRecepient;
        public L_Recepients()
        {
            collRecepient = new HashSet<L_Recepient>();
        }

        public L_Recepient AddRecepient(L_Recepient rp)
        {
            collRecepient.Add(rp);

            return rp;
        }

        public void Sort(string szvRName)
        {

            collRecepient.OrderByDescending(t => t.RecepientName);

        }

        public bool RecepientExists(string RecepientID)
        {
            L_Recepient rp = collRecepient.FirstOrDefault(rp => rp.RecepientId == RecepientID);

            if (rp == null)
            {
                return false;
            }

            return true;
        }

        public L_Recepient GetRecepient(string RecepientID)
        {
            L_Recepient rp = collRecepient.FirstOrDefault(rp => rp.RecepientId == RecepientID);

            return rp;
        }

        public IEnumerator<L_Recepient> GetEnumerator()
        {
            return collRecepient.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return collRecepient.GetEnumerator();
        }
        
        ~L_Recepients()
        {

        }
    }
}