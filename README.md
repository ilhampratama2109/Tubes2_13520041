## Tubes2_13520041
# **Tubes Stima 2 by DBFS**

Tugas Besar II IF2211 Strategi Algoritma Semester II Tahun 2020

**Pengaplikasian Algoritma BFS dan DFS dalam Implementasi _Folder Crawling_**

## Table of Contents
* [Introduction](#introduction)
* [General Information](#general-information)
* [Technologies Used](#technologies-used)
* [Installation and Requirement](#installation-and-requirement)
* [How to Run](#how-to-run)
* [How to Use](#how-to-use)
* [Project Status](#project-status)
* [Room for Improvement](#room-for-improvement)
* [Acknowledgements](#acknowledgements)
* [Contact](#contact)


## Introduction
Hai, Selamat datang di Repository Github kami!

Proyek ini dibuat oleh:
| No. | Nama | NIM |
| :---: | :---: | :---: |
| 1. | Ilham Pratama | 13520041 |
| 2. | Eiffel Aqila Amarendra | 13520074 |
| 3. | Raka Wirabuana Ninagan | 13520134 |


## General Information

Dalam tugas besar ini, kelompok kami membangun sebuah aplikasi GUI sederhana
yang dapat memodelkan fitur dari file explorer pada sistem operasi, yang pada tugas ini disebut
dengan _Folder Crawling_.

Penelusuran folder-folder yang ada pada direktori dilakukan dengan memanfaatkan algoritma _Breadth First Search_ (BFS) dan _Depth
First Search (DFS)_. Selain itu, kami juga membuat fitur untuk memvisualisasikan hasil dari
pencarian folder tersebut dalam bentuk pohon. Selain pohon, kami juga menampilkan list path dari daun-daun yang bersesuaian dengan
hasil pencarian. Path tersebut diharuskan memiliki hyperlink menuju folder parent dari file yang dicari, agar file langsung dapat diakses melalui browser atau file explorer.


## Technologies Used
Implementasi program ditulis dalam bahasa pemrograman C# dalam framework .NET.

## Installation and Requirement
- Unduh seluruh folder dan file pada repository ini atau clone repository
- Unduh dan pasang C#
- Unduh dan pasang [Visual Studio](https://visualstudio.microsoft.com/downloads/)
- Unduh dan pasang [MSAGL](https://github.com/microsoft/automatic-graph-layout)

## How to Run
- Pastikan semua requirement di atas sudah terpasang (install) pada perangkat keras yang akan digunakan
- Untuk menjalankan aplikasi DBFS dapat dilakukan dengan 2 buah cara, antara lain melalui file solution dengan Visual Studio dan melalui executable code.
### Via File Solution dengan Visual Studio
1. Jalankan Visual Studio yang telah terpasang sebelumnya.
2. Buka solution explorer dan pilih file DBFS.sln yang terletak pada folder src/DBFS pada repository ini pada Visual Studio.
3. Klik tombol "Start" pada panel atas dan pastikan DBFS sudah terpilih.
4. Visual Studio akan secara otomatis menjalankan proses build dan menjalankan aplikasi jika build berhasil.
### Via Executable Code
1. Pilih dan klik pada file DBFS.exe yang ada pada folder bin pada repository ini.

## How to Use
1. Jalankan aplikasi DBFS melalui file solution dengan Visual Studio atau melalui executable code.
2. Jika aplikasi berhasil dijalankan akan ditampilkan menu utama dari aplikasi DBFS.
3. Klik tombol "Change folder.." untuk memilih starting directory yang ingin digunakan sebagai direktori awal pencarian file.
4. Masukkan nama file yang ingin dicari beserta extension-nya.
5. Pilih method algorithm yang ingin digunakan dalam proses pencarian file tersebut, yakni menggunakan algoritma Breadth First Search (BFS) atau Depth First Search (DFS).
6. Jika ingin mencari setiap kemunculan file di dalam direktori tersebut, check checkbox "findAll".
7. Untuk memproses dan menampilkan hasil serta visualisasi langkah per langkah, klik tombol "Search". Hasil visualisasi, kemudian, akan tampil pada gviewer.
8. Untuk membuka lokasi ditemukannya file tersebut di dalam direktori, pilih salah satu hyperlink pada dropdown menu dan klik "Open file location". Jika ingin membuka hyperlink di browser, klik "Copy to Clipboard" untuk menyalin path dan tempel pada browser anda.
9. Untuk melihat waktu kerja algoritma, Anda dapat melihat pada textBox time taken (s).

## Project Status
Project status: _complete_

Seluruh spesifikasi dan bonus telah dibuat dan dipenuhi.


## Room for Improvement
- Sebuah algoritma program yang dapat membuat program berjalan lebih cepat dan efisien


## Acknowledgements
- Project ini dibuat berdasarkan [Spesifikasi Tugas Besar 2 Stima](https://cdn-edunex.itb.ac.id/38015-Algorithm-Strategies-Parallel-Class/85259-BFS-dan-DFS/1646201812962_Tugas-Besar-2-IF2211-Strategi-Algoritma-2022.pdf);
- Terima kasih kepada Tuhan Yang Maha Esa;
- Terima kasih kepada Ibu Masayu Leylia Khodra, Ibu Nur Ulfa Maulidevi, dan Pak Rinaldi sebagai dosen kami;
- Terima kasih kepada asisten;
- Project ini dibuat untuk memenuhi Tugas Besar 2 IF2211 Strategi Algoritma.


## Contact
Created by DBFS. 2022 All Rights Reserved.