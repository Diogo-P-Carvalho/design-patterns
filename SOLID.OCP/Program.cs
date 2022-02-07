using System;
using System.Collections.Generic;

namespace SOLID.OCP
{
    // OCP -> Open-Closed Principle
    // Classes should be open for extension but closed for modification

    enum Color
    {
        Red,
        Green,
        Blue
    }

    enum Size
    {
        Small,
        Medium,
        Large,
        Yuge
    }

    class Product
    {
        public string Name;
        public Color Color;
        public Size Size;

        public Product(string name, Color color, Size size)
        {
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            Color = color;
            Size = size;
        }
    }

    class ProductFilter
    {
        // let's suppose we don't want ad-hoc queries on products

        public IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
        {
            foreach (Product product in products)
            {
                if (product.Size == size)
                {
                    yield return product;
                }
            }
        }

        public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color)
        {
            foreach (Product product in products)
            {
                if (product.Color == color)
                {
                    yield return product;
                }
            }
        }

        public IEnumerable<Product> FilterBySizeAndColor(IEnumerable<Product> products, Size size, Color color)
        {
            foreach (Product product in products)
            {
                if (product.Size == size && product.Color == color)
                {
                    yield return product;
                }
            }
        }

        // state space explosion
        // 3 criteria = 7 methods

        // OCP = open for extension but closed for modification
    }

    // we introduce two new interfaces that are open for extension
    interface ISpecification<T>
    {
        bool IsSatisfied(T t);
    }

    interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }

    class ColorSpecification : ISpecification<Product>
    {
        private Color _color;

        public ColorSpecification(Color color)
            => _color = color;        

        public bool IsSatisfied(Product t)
        {
            return t.Color == _color;
        }
    }

    class SizeSpecification : ISpecification<Product>
    {
        private Size _size;

        public SizeSpecification(Size size)
            => _size = size;

        public bool IsSatisfied(Product t)
        {
            return t.Size == _size;
        }
    }

    // combinator
    class AndSpecification<T> : ISpecification<T>
    {
        ISpecification<T> _first, _second;

        public AndSpecification(ISpecification<T> first, ISpecification<T> second)
        {
            _first = first ?? throw new ArgumentNullException(paramName: nameof(first)); ;
            _second = second ?? throw new ArgumentNullException(paramName: nameof(second)); ;
        }

        public bool IsSatisfied(T t)
        {
            return _first.IsSatisfied(t) && _second.IsSatisfied(t);
        }
    }

    class BetterFilter : IFilter<Product>
    {
        public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
        {
            foreach (var item in items)
            {
                if (spec.IsSatisfied(item))
                {
                    yield return item;
                }
            }
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var apple = new Product("Apple", Color.Green, Size.Small);
            var tree = new Product("Tree", Color.Green, Size.Large);
            var house = new Product("House", Color.Blue, Size.Large);

            Product[] products = {apple, tree, house};

            var pf = new ProductFilter();
            Console.WriteLine("Green products (old):");
            foreach (var p in pf.FilterByColor(products, Color.Green))
            {
                Console.WriteLine($" - {p.Name} is green");
            }

            // ^^ BEFORE

            // vv AFTER
            var bf = new BetterFilter();
            Console.WriteLine("Green products (new):");
            foreach (var p in bf.Filter(products, new ColorSpecification(Color.Green)))
            {
                Console.WriteLine($" - {p.Name} is green");
            }

            Console.WriteLine("Large products (new):");
            foreach (var p in bf.Filter(products, new SizeSpecification(Size.Large)))
            {
                Console.WriteLine($" - {p.Name} is large");
            }

            Console.WriteLine("Large blue products (new):");
            foreach (var p in bf.Filter(products, new AndSpecification<Product>(new ColorSpecification(Color.Blue), new SizeSpecification(Size.Large))))
            {
                Console.WriteLine($" - {p.Name} is large and blue");
            }
        }
    }
}
