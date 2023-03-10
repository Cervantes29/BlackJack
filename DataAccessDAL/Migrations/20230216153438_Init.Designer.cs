// <auto-generated />
using DataAccessDAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccessDAL.Migrations
{
    [DbContext(typeof(GameDbContext))]
    [Migration("20230216153438_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ModelsEL.Gamelog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<float>("BalanceChange")
                        .HasColumnType("real");

                    b.Property<string>("DealerCards")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DealerScore")
                        .HasColumnType("int");

                    b.Property<bool>("Decision")
                        .HasColumnType("bit");

                    b.Property<string>("Outcome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PlayerCards")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PlayerScore")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Gamelogs");
                });

            modelBuilder.Entity("ModelsEL.PossibleDealerHands", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.ToTable("PossibleDealerHands");
                });

            modelBuilder.Entity("ModelsEL.PossiblePlayerHands", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.ToTable("PossiblePlayerHands");
                });

            modelBuilder.Entity("ModelsEL.Scenario", b =>
                {
                    b.Property<int>("ScenarioID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ScenarioID"), 1L, 1);

                    b.Property<string>("DealerHand")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Decision")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PlayerHand")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PossibleDealerHandsID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PossiblePlayerHandsID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Round")
                        .HasColumnType("int");

                    b.HasKey("ScenarioID");

                    b.HasIndex("PossibleDealerHandsID");

                    b.HasIndex("PossiblePlayerHandsID");

                    b.ToTable("Scenarios");
                });

            modelBuilder.Entity("ModelsEL.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AllTimeBalance")
                        .HasColumnType("int");

                    b.Property<int>("Balance")
                        .HasColumnType("int");

                    b.Property<float>("Decision")
                        .HasColumnType("real");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ModelsEL.Scenario", b =>
                {
                    b.HasOne("ModelsEL.PossibleDealerHands", null)
                        .WithMany("DealerHand")
                        .HasForeignKey("PossibleDealerHandsID");

                    b.HasOne("ModelsEL.PossiblePlayerHands", null)
                        .WithMany("PlayerHand")
                        .HasForeignKey("PossiblePlayerHandsID");
                });

            modelBuilder.Entity("ModelsEL.PossibleDealerHands", b =>
                {
                    b.Navigation("DealerHand");
                });

            modelBuilder.Entity("ModelsEL.PossiblePlayerHands", b =>
                {
                    b.Navigation("PlayerHand");
                });
#pragma warning restore 612, 618
        }
    }
}
