namespace AudioTAGEditor.Services
{
    public class EnergyConverter
    {
        public double CaloriesToJoules(double calories)
        {
            return calories * 4.1868;
        }

        public double JoulesToCalories(double joules)
        {
            return joules / 4.1868;
        }
    }
}
