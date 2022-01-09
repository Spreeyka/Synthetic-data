# Synthetic data

Console application written in C# .net core to generate fraudulent and fair transactions in separated CSV files. The goal of this is to check whether it is possible to detect anomalies (fraudulent activities) using Benford's law and machine learning algorithms. It is a part of my master’s thesis. Fair transactions are generated with obey of Benford's law distribution, while fraududulent uses 3 methods: Gauss, Random, and U-shaped to show potentially different approaches to manipulate data.

As result we have compliance parameters for further analysis with machine learning, which we can use for classification:
![image](https://user-images.githubusercontent.com/37092171/142734675-a00b8a50-877e-4a5b-94f6-604ac6fd3f26.png)
