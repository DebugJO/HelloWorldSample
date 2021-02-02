void Main()
{
	//new Duck().Eat(new Seeds());
	//new AnimalFeedingContext<Duck, Seeds, Pond>(new Duck()).Arrive().Feed(new Seeds());
	Factory.Create(new Duck()).Arrive().Feed(new Seeds());
}

public abstract class Animal<F, L> where F : Food where L : Location
{
	public void Eat(F food)
	{
		$"{this.GetType().Name} is eating {typeof(F).Name} at {typeof(L).Name}".Dump();
	}
}

public class Duck : Animal<Seeds, Pond>
{
}

public interface Food { }
public class Bread : Food { }
public class Seeds : Food { }

public interface Location { }
public class Pond : Location { }
public class Lake { }

public class Factory
{
	public static AnimalFeedingContext<Animal<F, L>, F, L> Create<F, L>(Animal<F, L> a) where F : Food where L : Location
	{
		return new AnimalFeedingContext<Animal<F, L>, F, L>(a);
	}
}

public class AnimalFeedingContext<A, F, L> where A : Animal<F, L> where F : Food where L : Location
{
	Animal<F, L> _animal;
	public AnimalFeedingContext(A animal)
	{
		_animal = animal;
	}

	public AnimalFeedingContext<A, F, L> Arrive()
	{
		$"Arrived at {typeof(L).Name}".Dump();
		return this;
	}

	public void Feed(F food)
	{
		_animal.Eat(food);
	}
}

// [Reference] : Raw Coding, "C# Generics Explained", https://www.youtube.com/watch?v=Q1Tv7vj3Txo
