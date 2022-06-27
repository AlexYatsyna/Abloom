# Abloom

## About The Project
**Abloom** is an app for estimating the complexity of finding a password from a hash, in conditions when all the details of hash generation (The algo and all the parameters of the algo) are known.

## Table of contents

- [About the project](#about-the-project)
- [Built with](#built-with)
- [Quick introduction to Akka.Cluster](#quick-introduction-to-akka-cluster)
- [Node hierarchy](#node-hierarchy)
- [Short Overview of Actors](#short-overview-of-actors)
- [Getting Started](#getting-started)
- [About Configuration files](#about-configuration-files)
- [Acknowledgments](#acknowledgments)


# Built With

* [Akka.NET](https://getakka.net/)
* [.NET](https://docs.microsoft.com/en-us/dotnet/)

# Quick introduction to Akka Cluster 

[Akka Cluster](https://getakka.net/articles/clustering/cluster-overview.html) provides great support to the creation of distributed applications. The best use case is when you have a node that you want to replicate N times in a distributed environment. This means that all the N nodes are peers running the same code. Akka Cluster gives you out-of-the-box the discovery of members in the same cluster. Using Cluster Aware Routers it is possible to balance the messages between actors in different nodes. It is also possible to choose the balancing policy, making load-balancing a piece of cake!

### Actually you can chose between two types of routers:

[**Group Router**](https://getakka.net/articles/clustering/cluster-routing.html) — The actors to send the messages to — called routees — are specified using their actor path. The routers share the routees created in the cluster. We will use a Group Router in this example.

![Group Router](/assets/grouprouter.jpg)

[**Pool Router**](https://getakka.net/articles/clustering/cluster-routing.html) — The routees are created and deployed by the router, so they are its children in the actor hierarchy. Routees are not shared between routers. This is ideal for a primary-replica scenario, where each router is the primary and its routees the replicas.

![Pool Router](/assets/poolrouter.jpg)

This is just the tip of the iceberg, so I invite you to read the [official documentation](https://getakka.net/articles/clustering/cluster-overview.html) for more insights.

# Node hierarchy

## First of all, let's look at the structure of cluster node.

### Node can be divided in three parts : business logic, cluster managment, node.

## Cluster managment 

To manage cluster we need a **`ClusterManager`**. This actor handles everything related to the cluster, like returning cluster members when asked or logging what happens inside the cluster, so define a **`ClusterListener`** actor. It's child of the **`ClusterManager`** and subscribes to cluster events logging them. 

## Business logic

App should do some computation. To do this, **`ProcessManager`** is defined that manage all requests for some actions. Each task, which need to support, is implemented in a specific actor, which is a child of **`ProcessManager`**. In this way App is modular and easier to extend.

## Node 

The **`Node`** actor is the root of my hierarchy. It's entry point of **Actor System**. The **`ProcessManager`** and the **`ClusterManager`** are children elements of **`Node`**, along with the **`CustomRouterManager`**. It's the load balancer of the system, distributing the load among Nodes.

# Short Overview of Actors

## In this case, two kind of nodes are implemented:
- ### A *manager* node who is responsible for receiving data from user to trigger password guessing and display progress.
- ### A *worker* node who is only responsible for verifying the password and hash, sending the result to *manager*. sends a message to *manager* node that he is ready to work. 

As mentioned above, **`ClusterManager`** handles everything related to the cluster, **`ClusterListener`** logs the joining of new nodes, the shutdown of old and the unreachable state.   **`ProcessManager`** manages all requests for some action. **`CustomRouterManager`** is the load balancer, which distributing the load among nodes with ***'managing-node'*** roles *_(To implement a working node)_*. The **`Node`**  is entry point.

## DisplayProcessor

###  This Actor stores the number of processed passwords and starts progress display.
```
private BigInteger NumberOfPassCombinations { get; set; } // Maximum number of passwords
private BigInteger CurrentNumberOfComb { get; set; } = 1; // Number of verified passwords

// Set initial data from user
case SetInitialData data:       
    NumberOfPassCombinations = data.NumberOfPassCombinations;
    PasswordLength = data.PasswordLength;
    break;

// After checking the password interval, increase the number of verified passwords
case SetCurrentCombination current:
    CurrentNumberOfComb += current.CurrentCombination;
    break;

// Start display progress
case "Display":
    timer = new Timer();
    timer.Interval = 3000;
    timer.Elapsed += DisplayInfo;
    timer.AutoReset = true;
    timer.Enabled = true;

```

## GetDataProcessor

### This requests the initial data and sends it to others.

## SendRecievePassProcessor

### This one requests a list of passwords from the **`PasswordGenerator`** actor for verification and sends it to *workers*. And also receives the result of the verification and implements further logic.

```
// Receiving a message from *worker* that he is ready to work and then sending data to it
case ReadyForChecking data:
    ****

    // Request to get new intervals, when the previous ones have ended  
    if (PreparedToSend.IsEmpty)
    {
        PasswordgeneratorRef.Tell(new GetPasswordsIntervals(10, 25));
        isRequested = true;
    }

    ****
    break;

// Getting new password intervals (non-blocking Receive method)
 case RespondPasswordIntervals data:
    Task.Run(() =>
    {
        foreach (var item in data.Intervals)
        {
            PreparedToSend.GetOrAdd(Guid.NewGuid(), item);
        }
        isRequested = false;
    });
    break;

// Getting response from working node
 case RespondPassword data:
    ****
    break;


```

## PasswordGenerator

### Generates the requested number of itervals with a certain number of passwords. Then sends to **`SendReceivePassProcessor`**.

```
// Set initial data from user
case SetInitialData data:
    ****
    break;

// Generate intervals and send to SendReceivePassProcessor 
case GetPasswordsIntervals data:
    ****
        var result = GetPasswordIntervals(data.NumberOfIntervals, data.NumebrOfPasswordsInTheInterval);
        Sender.Tell(new RespondPasswordIntervals(result));
    ****
    break;
```

## PasswordCheckerProcessor

### This is an actor on the *worker* node that checks passwords and sends result of checking to the *managing* node.

```
// Checking passwords from interval, then reply to 'managing' node, and send 'ready for work' message to 
all 'managing' nodes 
case SendToWorkinNode data:
    var result = StartCheck(data);
    data.ReplyTo.Tell(result);
    Context.Parent.Tell("Ready for checking");
    break;
```

#  Getting Started

## 1. Git clone
```
    git clone https://github.com/AlexYatsyna/Abloom
```
## 2. Install 

### If you want to include Akka.NET in your project, you can install it directly from NuGet. You need to install Akka.NET, Akka.Cluster.
### or run the following command in the Package Manager Console
```
PM> Install-Package Akka
PM> Install-Package Akka.Cluster
```

## 3. Add the Hashers folder with password verifying file *(Like BCrypt.Verify hash)* to the *worker* node.

## 4. Configure 'App.conf' files using HOCON

# About Configuration files

## Actor block:
Parameter   | Description
----------- | -----------
provider |"Akka.Cluster.ClusterActorRefProvider, Akka.Cluster", FQCN of the ActorRefProvider to be used; the below is the built-in default, another one is akka.remote.RemoteActorRefProvider in the akka-remote bundle.
creation-timeout|Timeout for ActorSystem.actorOf
 reaper-interval | Frequency with which stopping actors are prodded in case they had to be removed from their parents
 serialize-messages | Serializes and deserializes (non-primitive) messages to ensure immutability
 serialize-creators | Serializes and deserializes creators (in Props) to ensure that they can be sent over the network
 ask-timeout | Timeout for IActorRef.Ask
 serializers  | Entries for pluggable serializers and their bindings.
serialization-bindings |Class to Serializer binding. You only need to specify the name of an interface or abstract base class of the messages. In case of ambiguity it is using the most specific configured class, or giving a warning and choosing the “first” one.

 ### deployment block:
Parameter   | Description
----------- | -----------
router | Routing (load-balance) scheme to use available: "from-code", "round-robin", "random", "smallest-mailbox", "scatter-gather", "broadcast"  or:        Fully qualified class name of the router class.
nr-of-instances | Number of children to create in case of a router, this setting is ignored if routees.paths is given
routees.paths | Alternatively to giving nr-of-instances you can specify the full paths of those actors which should be routed to. This setting takes precedence over nr-of-instances
resizer | Dynamically resizable number of routees as specified in resizer below
within | Within is the timeout used for routers containing future calls

## More Info [Configuration](https://getakka.net/articles/configuration/modules/akka.html).

## In this App need to describe the following blocks:
- ### Actor
- ### Actor.deployment *(for routers)*
- ### Remote
- ### Cluster   

# Acknowledgments

- ### [Akka.NET](https://getakka.net/index.html)
- ### [Petabridge](https://petabridge.com/)
- ### [Akka.NET Live Webinar](https://www.youtube.com/watch?v=8PenRoEjZKc)