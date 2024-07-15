namespace IKVM.Tests.Java.javax.script
{

    public class Person
    {

        public int id = 0;

        public Person()
        {

        }

        public Person(int code)
        {
            this.id = code;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Person)
            {
                Person o = (Person)obj;
                return this.id == o.id;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return id;
        }

        public override string ToString()
        {
            return "Person(" + id + ")";
        }

    }

}
