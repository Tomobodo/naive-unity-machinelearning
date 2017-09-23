using System.Collections.Generic;

public class TrainingComparer : IComparer<Training>
{
	public int Compare (Training a, Training b)
    {
        if(a.fitness < b.fitness)
        {
            return 1;
        } if (b.fitness < a.fitness)
        {
            return -1;
        } else
        {
            return 0;
        }
    }
}
