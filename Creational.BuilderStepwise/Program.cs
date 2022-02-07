using System;

namespace Creational.Builder.Stepwise
{
    enum CarType
    {
        Sedan,
        Crossover
    }

    class Car
	{
        public CarType Type;
        public int WheelSize;
	}

    interface ISpecifyCarType
    {
        ISpecifyWheelSize OfType(CarType type);
    }

    interface ISpecifyWheelSize
    {
        IBuildCar WithWheels(int size);
    }

    interface IBuildCar
    {
        Car Build();
    }

    class CarBuilder
    {
        private class Impl : ISpecifyCarType, ISpecifyWheelSize, IBuildCar
        {  
            private Car car = new Car();

            public ISpecifyWheelSize OfType(CarType type)
            {
                car.Type = type;
                return this;
            }

            public IBuildCar WithWheels(int size)
            {
                switch (car.Type)
                {
                    case CarType.Crossover when size < 17 || size > 20:                        
                    case CarType.Sedan when size < 15 || size > 17:
                        throw new ArgumentException($"Wrong size of wheel for {car.Type}");
                }

                car.WheelSize = size;
                return this;
            }

            public Car Build()
            {
                return car;
            }
        }

        public static ISpecifyCarType Create()
        {
            return new Impl();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var car = CarBuilder.Create()  // ISpecifyCarType
                .OfType(CarType.Crossover) // ISpecifyWhellSize
                .WithWheels(18)       // IBuidCar
                .Build();
        }
    }
}
