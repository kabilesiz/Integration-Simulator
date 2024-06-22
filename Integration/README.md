# Packages

| Package | Description |
| ------ | ------ |
| BenchmarkDotNet | Package for conducting benchmark tests |
| Bogus | Package for generating items |
| StackExchange.Redis | Package for Redis connection and operations |

---

# Requirements

Before running the simulator, make sure Docker is installed on your computer.

```sh
docker -v 
```
You can run the command above to check if Docker is installed. If it is, you should see an output like 

```sh
Docker version x.x.x
```
If you do not see this output, you can download Docker from  https://www.docker.com/products/docker-desktop/

## Before Running
You can run this command in the root directory. This will create a local Redis image on your local docker, which will be used for the Distributed System Scenario.

```sh
docker-compose up -d
```
After running this, you will see a log like this.

![Compose Log](https://i.hizliresim.com/9kcvxk7.png)

---

# Running
![Menu](https://i.hizliresim.com/att8jlx.png)

In the menu, you can perform the operations indicated by the numbers

---

### 1: Single Server Scenario

![Generated Item Menu](https://i.hizliresim.com/cxgiqgy.png)

The first question asks how many items you want to test with. You must enter a value greater than 0. (These items/texts will be generated using the Bogus library)

The second question asks how many times you want a specific item to repeat. (For example: If you enter 30 for the first question and AdCreativeAI:8 for the second question, 8 AdCreativeAI and 22 Bogus-generated words/items will be given to the simulation. There is no guarantee that items generated with Bogus will be unique!)

### Note: You can check how many unique items there are by comparing the entry and final order tables. For more details, see the Log Traces section.

---

### 2: Distributed System Scenario

![Clear Redis](https://i.hizliresim.com/hvzq8fa.png)

In this scenario, after answering the question of whether you want to clear the data in Redis, you will proceed to the item generate menu (like Single Server Scenario).

---


### 3: Benchmark test

With this option, you can see the differences between these two scenarios. Make sure you are in Release mode for this option. You can make changes in the BenchmarkConfig class to adjust the settings in the benchmark. If you want to see the results directly without running the benchmark tests, you can view the results in the benchmark.txt file.

---

### Log Traces

We can understand the parallelism by looking at the entry and final order of an item's id/index in the process. This can be seen in two ways

---

##### i: Console Log Traces

![Console Log Trace](https://i.hizliresim.com/jm11his.png)

You can track an item's process entry order and final order through the logs, but it will be difficult to follow when the number of items increases. Therefore, it will be easier to track using the table structure provided as the second option.

---

##### ii: Table Traces
![Console Log Trace](https://i.hizliresim.com/7bz9t8w.png)

The first table, ENTRYORDER, shows the entry order of an item, and the second table, FINALORDER, shows the final order of an item.



