using DAL;

namespace Project
{
    class Program
    {
        static void Main(string[] args)
        {
            // The second 15 means how much time since starting the program invoking the timer, the first 15 means hwow much time between invokes.
            UI ui = new UI(15, 15,20);
            ui.StartProgram();
            // Im aware of the Extreme case that if we try to draw and between the suggested result the self check delete the item and then we accept the order
            // we will recive new items not the same as the suggested ones. I tried to fixe it but this option could happent once a mounth, and the program
            // will bring acceptable boxes that will still fit.
        }
    }
}
