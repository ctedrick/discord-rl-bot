// <auto-generated />
using CodyTedrick.DiscordBot.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CodyTedrick.Migrations
{
    [DbContext(typeof(CsharpiEntities))]
    partial class CsharpiEntitiesModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0-preview.1.23111.4");

            modelBuilder.Entity("CodyTedrick.DiscordBot.Database.UserInfo", b =>
                {
                    b.Property<ulong>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId");

                    b.ToTable("UserInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
