# Blue Prism Technical Assessment

### First thoughts... ###

(Cribbed from sticky notes!)

Multi-step problem: transforming words to next word, looking for match of each possible next word in supplied dictionary.  Recursion, possible BFS solution, similar to travelling salesman problem.  Should we build graph of all possible paths or can we just run BFS until matched end word?  Latter should be faster.

Word transform -> Iterate over each char, iterate over alphabet replacing one char at a time and looking for new word in dictionary

Supplied dictionary includes 4 letter words as expected but also shorter and longer words, acronyms, non-english words, names etc.  We can filter out non-required words maybe using dictionary API but this would slow down overall process.  Dictionary may not be sorted initially.

Edge cases-> no matches/path, multiple chains of same length (stop on first matched anyway and return result)

Async -> parallelisation of scanning - maybe separate threads in parallel for each character replacement in word - max threads configurable?  Parallel.ForEach with maxDegreeOfParallelism???

Will need to search for each possible new word in list - what is fastest way to do this? Could use string array, arraylist, list<string>, sorted list, dictionary, hashtable.  Fastest way to search: LINQ e.g. .Contains vs custom binary search etc. 

Recursive part of BFS - need to track chain of matched words to return as result

### Approach ###

1) Requested clarification from Blue Prism relating to input data concerns. After the response and following further consideration, I made following assumptions about the data provided in words-english.txt:
    * Include only proper 4-letter english words from the list provided, e.g. spin, Spot etc
    * Do not include acronyms e.g. ACME, AT&T etc
    * Do not include words containing non-alpha characters e.g. /4th, /5th, 22nd
    * Words returned in results should be returned in output in the same case as they are found in the file. So for example, when using the sample file words-english.txt and providing start word 'amos' and end word 'spot', output will include 'Amos' and 'spot' (note case) as per the spelling in the sample file

2) Some further assumptions: 
    * If more than one path with the same number of steps exists for a given start word and end word then it should be ok to return either
    * Result will always be written to the console but also to output file if ouput file name is provided at command line
    * Output file will be overwritten for each program execution

3) Created some CRC cards as a rough outline to facilitate OO design: https://echeung.me/crcmaker/?share=W3sibmFtZSI6IldvcmQgTGlzdCIsInN1cGVyY2xhc3NlcyI6IiIsInN1YmNsYXNzZXMiOiIiLCJ0eXBlIjoxLCJyZXNwb25zaWJpbGl0aWVzIjpbIkxvYWQgZGljdGlvbmFyeSB3b3JkcyBmcm9tIGZpbGUiLCJGaW5kIHNpbWlsYXIgd29yZHMgaW4gZGljdGlvbmFyeSIsIiJdLCJjb2xsYWJvcmF0b3JzIjpbIldvcmQgVmFsaWRhdG9yIl19LHsibmFtZSI6IldvcmQgVmFsaWRhdG9yIiwic3VwZXJjbGFzc2VzIjoiIiwic3ViY2xhc3NlcyI6IiIsInR5cGUiOjEsInJlc3BvbnNpYmlsaXRpZXMiOlsiVmFsaWRhdGUgV29yZCJdLCJjb2xsYWJvcmF0b3JzIjpbIldvcmQgTGlzdCJdfSx7Im5hbWUiOiJQYXRoIEZpbmRlciIsInN1cGVyY2xhc3NlcyI6IiIsInN1YmNsYXNzZXMiOiIiLCJ0eXBlIjoxLCJyZXNwb25zaWJpbGl0aWVzIjpbIkZpbmQgc2hvcnRlc3QgcGF0aCJdLCJjb2xsYWJvcmF0b3JzIjpbIldvcmQgTGlzdCJdfV0=

    Note: This was a reasonable starting point but I ended up refactoring later into different classes as the problem solution was better understood

4) Developed ideas to solve the crux of the problem. I knew from the outset that a breadth first search (BFS) based on a FIFO queue  was a good candidate having implemented a similar pattern previously, but was undecided on how to implement this in such a way that would track the success path.  I solved this by using a simple linked list of parent -> child nodes to track the connected words at each level while allowing the path back to the start word to be walked once the end word was found.

5) Considered approach to performance.  There were a few potential performance bottlenecks:
    * loading of the dictionary from the input file: minimised by  ensuring this only happened once per program execution
    * finding words in the dictionary while validating input args start and end words: I opted to load a sorted array of strings from the input file and then binary searched when looking for matching words
    * finding similar words to the current word whilst moving through the BFS: this was addressed by parallelising searches for next words for each char position in the current word and then also parallelising iteration of all alphabetical characters to determine the next word to search for in the dictionary (using find described above).  This should create a max of 4 x 2 threads plus the main thread for the typical input of 4 letter words and should perform reasonably well on most systems

    This resource provides a useful comparison of C# collection types in relation to seeking keys and values: https://cc.davelozinski.com/c-sharp/fastest-collection-for-string-lookups

6) Followed a partial TDD approach which based on experience I find to be most appropriate in this type of scenario.  The code is 100% unit-test covered and there are a small number of integration tests which validate real inputs and outputs

### Running the code... ###

Clone the repo and then cd into the /BluePrism.TechTest directory

From here run the following command to execute the program replacing the command line args as appropriate...

```sh
    dotnet run -i <input file name> -s <startWord> -e <endWord> -o <output file name>
```

### Example ###

For sample file, words-english.txt, find path from 'amos' to 'spot'.....

```sh
    dotnet run -i words-english.txt -s amos -e spot -o output.txt
```

...produces result...

```sh
    Success! Shortest path found from amos to spot is: Amos -> Ames -> Ares -> area -> aria -> arid -> grid -> grad -> goad -> good -> rood -> root -> soot -> spot
```