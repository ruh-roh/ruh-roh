[![Build Status](https://gotsharp.visualstudio.com/Ruh-Roh/_apis/build/status/Ruh-Roh-CI?branchName=master)](https://gotsharp.visualstudio.com/Ruh-Roh/_build/latest?definitionId=5?branchName=master)

# Ruh Roh
Ruh Roh is what you call a chaos monkey. You can use it to test your application under certain circumstances where everything that can go wrong, will go wrong, things often difficult to test otherwhise.

For example: when developing, that external API always responds in time, but in production, the same API takes too much time to respond or doesn't respond at all. Using Ruh Roh, you can mimic this behavior at random, at certain moments in time, or when you call a certain method for the n-th time.

Another example: you have to process files to disk and want to simulate how your application would deal with the situation when the disk is full. Ruh Roh allows you to do this by throwing exceptions like System.IO.IOException, again when you want it to (every time, the n-th time, after n calls, between certain times, ...)

Ruh Roh is still in development however, so stay tuned for the first release!

This project uses [GitHub flow](https://guides.github.com/introduction/flow/).
