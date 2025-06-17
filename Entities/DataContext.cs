
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EF_MVC.Models
{
    public class DataContext : IdentityDbContext<User, Role, int>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<CardItem> CardItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)//migrations çalıştığında bu methotta çalışır
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().Property(p => p.Discount).HasDefaultValue(0);
            modelBuilder.Entity<Slider>().HasData
            (
                new List<Slider>
                {
                    new Slider{Id=1,Title="Slider 1 Başlık",Description="Slider 1 Açıklama",img="slider-1.jpeg",Index=0,isActive=true},
                    new Slider{Id=2,Title="Slider 2 Başlık",Description="Slider 2 Açıklama",img="slider-2.jpeg",Index=1,isActive=true},
                    new Slider{Id=3,Title="Slider 3 Başlık",Description="Slider 3 Açıklama",img="slider-3.jpeg",Index=2,isActive=true}
                }
            );
            modelBuilder.Entity<Category>().HasData
            (
                new List<Category>()
                {
                    new Category{Id=1,Name="Saat",URL="saat"},
                    new Category{Id=2,Name="Telefon",URL="telefon"},
                    new Category{Id=3,Name="Giyim",URL="giyim"},
                    new Category{Id=4,Name="Spor",URL="spor"},
                    new Category{Id=5,Name="Kitap",URL="kitap"},
                    new Category{Id=6,Name="Anne/Bebek",URL="anne-bebek"},
                    new Category{Id=7,Name="Aksesuar",URL="aksesuar"},
                    new Category{Id=8,Name="Bilgisayar",URL="bilgisayar"},
                    new Category{Id=9,Name="Tablet",URL="tablet"},
                    new Category{Id=10,Name="Elektronik",URL="elektronik"},



                }
            );
            modelBuilder.Entity<Product>().HasData
            (
                new List<Product>(){
                   new Product() { Id = 1, Name="Apple Watch 7", Price=10000, isActive=true,img="1.jpeg",Description="Lorem ipsum dolor sit amet consectetur adipisicing elit. Fugit cumque est vero voluptates maiores praesentium quaerat incidunt aspernatur. Delectus deserunt porro velit est dolor ullam autem exercitationem nemo, rem iure.",isHome=false,CategoryId=1},

                new Product() { Id = 2, Name="Apple Watch 8", Price=20000, isActive=false,img="2.jpeg",Description="Lorem ipsum dolor sit amet consectetur adipisicing elit. Fugit cumque est vero voluptates maiores praesentium quaerat incidunt aspernatur. Delectus deserunt porro velit est dolor ullam autem exercitationem nemo, rem iure.",isHome=true,CategoryId=1},

                new Product() { Id = 3, Name="Apple Watch 9", Price=30000, isActive=true,img="3.jpeg",Description="Lorem ipsum dolor sit amet consectetur adipisicing elit. Fugit cumque est vero voluptates maiores praesentium quaerat incidunt aspernatur. Delectus deserunt porro velit est dolor ullam autem exercitationem nemo, rem iure.",isHome=true,CategoryId=1},

                new Product() { Id = 4, Name="Apple Watch 10", Price=40000, isActive=false,img="4.jpeg",Description="Lorem ipsum dolor sit amet consectetur adipisicing elit. Fugit cumque est vero voluptates maiores praesentium quaerat incidunt aspernatur. Delectus deserunt porro velit est dolor ullam autem exercitationem nemo, rem iure.",isHome=true,CategoryId=1},

                new Product() { Id = 5, Name="Apple Watch 11", Price=50000, isActive=true,img="5.jpeg",Description="Lorem ipsum dolor sit amet consectetur adipisicing elit. Fugit cumque est vero voluptates maiores praesentium quaerat incidunt aspernatur. Delectus deserunt porro velit est dolor ullam autem exercitationem nemo, rem iure.",isHome=true,CategoryId=2},

                new Product() { Id = 6, Name="Iphone 6", Price=10000, isActive=true,img="6.jpeg",Description="Lorem ipsum dolor sit amet consectetur adipisicing elit. Fugit cumque est vero voluptates maiores praesentium quaerat incidunt aspernatur. Delectus deserunt porro velit est dolor ullam autem exercitationem nemo, rem iure.",isHome=true,CategoryId=2},

                //  new Product() { Id = 7, Name="Iphone 13", Price=60000, isActive=true,img="7.jpeg",Description="Lorem ipsum dolor sit amet consectetur adipisicing elit. Fugit cumque est vero voluptates maiores praesentium quaerat incidunt aspernatur. Delectus deserunt porro velit est dolor ullam autem exercitationem nemo, rem iure.",isHome=true,CategoryId=2},

                //   new Product() { Id = 8, Name="Iphone 11", Price=90000, isActive=true,img="8.jpeg",Description="Lorem ipsum dolor sit amet consectetur adipisicing elit. Fugit cumque est vero voluptates maiores praesentium quaerat incidunt aspernatur. Delectus deserunt porro velit est dolor ullam autem exercitationem nemo, rem iure.",isHome=false,CategoryId=2},

                   new Product() { Id = 9, Name="Apple Watch 12", Price=60000, isActive=true,img="1.jpeg",Description="Lorem ipsum dolor sit amet consectetur adipisicing elit. Fugit cumque est vero voluptates maiores praesentium quaerat incidunt aspernatur. Delectus deserunt porro velit est dolor ullam autem exercitationem nemo, rem iure.",isHome=false,CategoryId=3},

                    new Product() { Id = 10, Name="Apple Watch 12", Price=60000, isActive=true,img="2.jpeg",Description="Lorem ipsum dolor sit amet consectetur adipisicing elit. Fugit cumque est vero voluptates maiores praesentium quaerat incidunt aspernatur. Delectus deserunt porro velit est dolor ullam autem exercitationem nemo, rem iure.",isHome=false,CategoryId=4},
                    }

            );
        }
    }
}