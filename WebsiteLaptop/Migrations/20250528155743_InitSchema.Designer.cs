﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebsiteLaptop.Data;

#nullable disable

namespace WebsiteLaptop.Migrations
{
    [DbContext(typeof(WebsiteLaptopContext))]
    [Migration("20250528155743_InitSchema")]
    partial class InitSchema
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebsiteLaptop.Models.ChiTietGioHang", b =>
                {
                    b.Property<int>("MaChiTietGioHang")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaChiTietGioHang"));

                    b.Property<int>("MaLaptop")
                        .HasColumnType("int");

                    b.Property<int>("MaNguoiDung")
                        .HasColumnType("int");

                    b.Property<DateTime>("NgayThem")
                        .HasColumnType("datetime2");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.HasKey("MaChiTietGioHang");

                    b.HasIndex("MaLaptop");

                    b.HasIndex("MaNguoiDung");

                    b.ToTable("ChiTietGioHang");
                });

            modelBuilder.Entity("WebsiteLaptop.Models.DanhMuc", b =>
                {
                    b.Property<int>("MaDanhMuc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaDanhMuc"));

                    b.Property<string>("MoTa")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("TenDanhMuc")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("MaDanhMuc");

                    b.ToTable("DanhMuc");
                });

            modelBuilder.Entity("WebsiteLaptop.Models.DonHang", b =>
                {
                    b.Property<int>("MaDonHang")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaDonHang"));

                    b.Property<string>("DiaChiGiaoHang")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("DienThoaiGiaoHang")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("MaNguoiDung")
                        .HasColumnType("int");

                    b.Property<DateTime>("NgayDatHang")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("TongTien")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TrangThai")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("MaDonHang");

                    b.HasIndex("MaNguoiDung");

                    b.ToTable("DonHang");
                });

            modelBuilder.Entity("WebsiteLaptop.Models.DonHangChiTiet", b =>
                {
                    b.Property<int>("MaChiTietDonHang")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaChiTietDonHang"));

                    b.Property<decimal>("DonGia")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("MaDonHang")
                        .HasColumnType("int");

                    b.Property<int>("MaLaptop")
                        .HasColumnType("int");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.HasKey("MaChiTietDonHang");

                    b.HasIndex("MaDonHang");

                    b.HasIndex("MaLaptop");

                    b.ToTable("DonHangChiTiet");
                });

            modelBuilder.Entity("WebsiteLaptop.Models.Laptop", b =>
                {
                    b.Property<int>("MaLaptop")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaLaptop"));

                    b.Property<string>("DuongDanAnh")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<decimal>("Gia")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("HangSanXuat")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("MaDanhMuc")
                        .HasColumnType("int");

                    b.Property<string>("MoTa")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NgayTao")
                        .HasColumnType("datetime2");

                    b.Property<string>("TenDangNhap")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenLaptop")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("MaLaptop");

                    b.HasIndex("MaDanhMuc");

                    b.ToTable("Laptop");
                });

            modelBuilder.Entity("WebsiteLaptop.Models.NguoiDung", b =>
                {
                    b.Property<int>("MaNguoiDung")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaNguoiDung"));

                    b.Property<string>("DiaChi")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("DienThoai")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("HoVaTen")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("MatKhau")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("TenDangNhap")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("VaiTro")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("MaNguoiDung");

                    b.ToTable("NguoiDung");
                });

            modelBuilder.Entity("WebsiteLaptop.Models.ChiTietGioHang", b =>
                {
                    b.HasOne("WebsiteLaptop.Models.Laptop", "Laptop")
                        .WithMany("ChiTietGioHangs")
                        .HasForeignKey("MaLaptop")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebsiteLaptop.Models.NguoiDung", "NguoiDung")
                        .WithMany("ChiTietGioHangs")
                        .HasForeignKey("MaNguoiDung")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Laptop");

                    b.Navigation("NguoiDung");
                });

            modelBuilder.Entity("WebsiteLaptop.Models.DonHang", b =>
                {
                    b.HasOne("WebsiteLaptop.Models.NguoiDung", "NguoiDung")
                        .WithMany("DonHangs")
                        .HasForeignKey("MaNguoiDung")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NguoiDung");
                });

            modelBuilder.Entity("WebsiteLaptop.Models.DonHangChiTiet", b =>
                {
                    b.HasOne("WebsiteLaptop.Models.DonHang", "DonHang")
                        .WithMany("DonHangChiTiets")
                        .HasForeignKey("MaDonHang")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebsiteLaptop.Models.Laptop", "Laptop")
                        .WithMany("DonHangChiTiets")
                        .HasForeignKey("MaLaptop")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DonHang");

                    b.Navigation("Laptop");
                });

            modelBuilder.Entity("WebsiteLaptop.Models.Laptop", b =>
                {
                    b.HasOne("WebsiteLaptop.Models.DanhMuc", "DanhMuc")
                        .WithMany("Laptops")
                        .HasForeignKey("MaDanhMuc")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DanhMuc");
                });

            modelBuilder.Entity("WebsiteLaptop.Models.DanhMuc", b =>
                {
                    b.Navigation("Laptops");
                });

            modelBuilder.Entity("WebsiteLaptop.Models.DonHang", b =>
                {
                    b.Navigation("DonHangChiTiets");
                });

            modelBuilder.Entity("WebsiteLaptop.Models.Laptop", b =>
                {
                    b.Navigation("ChiTietGioHangs");

                    b.Navigation("DonHangChiTiets");
                });

            modelBuilder.Entity("WebsiteLaptop.Models.NguoiDung", b =>
                {
                    b.Navigation("ChiTietGioHangs");

                    b.Navigation("DonHangs");
                });
#pragma warning restore 612, 618
        }
    }
}
