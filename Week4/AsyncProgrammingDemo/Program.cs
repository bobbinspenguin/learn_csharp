using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncProgrammingDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Asynchronous Programming Demo ===\n");

            bool continueProgram = true;
            while (continueProgram)
            {
                Console.WriteLine("Choose a demonstration:");
                Console.WriteLine("1. Basic Async/Await");
                Console.WriteLine("2. Task.Run vs async/await");
                Console.WriteLine("3. Multiple Async Operations");
                Console.WriteLine("4. Error Handling in Async");
                Console.WriteLine("5. Cancellation Tokens");
                Console.WriteLine("6. ConfigureAwait Demo");
                Console.WriteLine("7. Async File Operations");
                Console.WriteLine("8. HTTP Client Async");
                Console.WriteLine("9. Producer-Consumer Pattern");
                Console.WriteLine("10. Async Performance Comparison");
                Console.WriteLine("11. Exit");
                Console.Write("\nEnter your choice (1-11): ");

                string choice = Console.ReadLine() ?? "";
                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            await BasicAsyncAwaitDemo();
                            break;
                        case "2":
                            await TaskRunVsAsyncDemo();
                            break;
                        case "3":
                            await MultipleAsyncOperationsDemo();
                            break;
                        case "4":
                            await ErrorHandlingDemo();
                            break;
                        case "5":
                            await CancellationTokenDemo();
                            break;
                        case "6":
                            await ConfigureAwaitDemo();
                            break;
                        case "7":
                            await FileOperationsDemo();
                            break;
                        case "8":
                            await HttpClientDemo();
                            break;
                        case "9":
                            await ProducerConsumerDemo();
                            break;
                        case "10":
                            await PerformanceComparisonDemo();
                            break;
                        case "11":
                            continueProgram = false;
                            Console.WriteLine("Goodbye!");
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                if (continueProgram)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        static async Task BasicAsyncAwaitDemo()
        {
            Console.WriteLine("--- Basic Async/Await Demo ---");

            Console.WriteLine("Starting synchronous operation...");
            var sw = Stopwatch.StartNew();
            
            // Synchronous version
            SynchronousOperation("Task 1");
            SynchronousOperation("Task 2");
            SynchronousOperation("Task 3");
            
            sw.Stop();
            Console.WriteLine($"Synchronous total time: {sw.ElapsedMilliseconds}ms\n");

            Console.WriteLine("Starting asynchronous operations...");
            sw.Restart();

            // Asynchronous version
            await AsynchronousOperation("Task 1");
            await AsynchronousOperation("Task 2");
            await AsynchronousOperation("Task 3");

            sw.Stop();
            Console.WriteLine($"Asynchronous total time: {sw.ElapsedMilliseconds}ms");

            // Note: Sequential async calls don't improve performance
            // They're useful for not blocking the calling thread
        }

        static void SynchronousOperation(string taskName)
        {
            Console.WriteLine($"{taskName} started");
            Thread.Sleep(1000); // Simulate work
            Console.WriteLine($"{taskName} completed");
        }

        static async Task AsynchronousOperation(string taskName)
        {
            Console.WriteLine($"{taskName} started");
            await Task.Delay(1000); // Simulate async work
            Console.WriteLine($"{taskName} completed");
        }

        static async Task TaskRunVsAsyncDemo()
        {
            Console.WriteLine("--- Task.Run vs async/await Demo ---");

            var sw = Stopwatch.StartNew();

            // Using Task.Run for CPU-bound work
            Console.WriteLine("Starting CPU-bound task with Task.Run...");
            var cpuTask = Task.Run(() => CpuBoundWork("CPU Task"));
            
            // Using async/await for I/O-bound work
            Console.WriteLine("Starting I/O-bound task with async/await...");
            var ioTask = IoLatched ("I/O Task");

            // Wait for both to complete
            await Task.WhenAll(cpuTask, ioTask);

            sw.Stop();
            Console.WriteLine($"Both tasks completed in: {sw.ElapsedMilliseconds}ms");

            Console.WriteLine("\nKey Difference:");
            Console.WriteLine("• Task.Run: Creates a new thread for CPU-bound work");
            Console.WriteLine("• async/await: Doesn't create new threads for I/O-bound work");
        }

        static int CpuBoundWork(string taskName)
        {
            Console.WriteLine($"{taskName} started on Thread {Thread.CurrentThread.ManagedThreadId}");
            
            // Simulate CPU-intensive work
            int result = 0;
            for (int i = 0; i < 100_000_000; i++)
            {
                result += i % 1000;
            }
            
            Console.WriteLine($"{taskName} completed on Thread {Thread.CurrentThread.ManagedThreadId}");
            return result;
        }

        static async Task<string> IoLatched (string taskName)
        {
            Console.WriteLine($"{taskName} started on Thread {Thread.CurrentThread.ManagedThreadId}");
            
            // Simulate I/O-bound work
            await Task.Delay(2000);
            
            Console.WriteLine($"{taskName} completed on Thread {Thread.CurrentThread.ManagedThreadId}");
            return $"{taskName} result";
        }

        static async Task MultipleAsyncOperationsDemo()
        {
            Console.WriteLine("--- Multiple Async Operations Demo ---");

            // Sequential execution
            Console.WriteLine("1. Sequential execution:");
            var sw = Stopwatch.StartNew();
            
            await AsynchronousOperation("Sequential 1");
            await AsynchronousOperation("Sequential 2");
            await AsynchronousOperation("Sequential 3");
            
            sw.Stop();
            Console.WriteLine($"Sequential time: {sw.ElapsedMilliseconds}ms\n");

            // Parallel execution with Task.WhenAll
            Console.WriteLine("2. Parallel execution with Task.WhenAll:");
            sw.Restart();

            var task1 = AsynchronousOperation("Parallel 1");
            var task2 = AsynchronousOperation("Parallel 2");
            var task3 = AsynchronousOperation("Parallel 3");

            await Task.WhenAll(task1, task2, task3);

            sw.Stop();
            Console.WriteLine($"Parallel time: {sw.ElapsedMilliseconds}ms\n");

            // Task.WhenAny - complete when first task finishes
            Console.WriteLine("3. Task.WhenAny - first to complete:");
            sw.Restart();

            var slowTask = Task.Delay(3000).ContinueWith(_ => "Slow task");
            var fastTask = Task.Delay(1000).ContinueWith(_ => "Fast task");
            var mediumTask = Task.Delay(2000).ContinueWith(_ => "Medium task");

            var completedTask = await Task.WhenAny(slowTask, fastTask, mediumTask);
            var result = await completedTask;

            sw.Stop();
            Console.WriteLine($"First completed: {result} in {sw.ElapsedMilliseconds}ms");

            // Wait for remaining tasks to complete
            await Task.WhenAll(slowTask, fastTask, mediumTask);
            Console.WriteLine("All tasks completed");
        }

        static async Task ErrorHandlingDemo()
        {
            Console.WriteLine("--- Error Handling in Async Demo ---");

            // Single async operation with error handling
            Console.WriteLine("1. Single operation error handling:");
            try
            {
                await OperationThatMightFail("Operation 1", false);
                Console.WriteLine("Operation 1 succeeded");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Caught exception: {ex.Message}");
            }

            try
            {
                await OperationThatMightFail("Operation 2", true);
                Console.WriteLine("Operation 2 succeeded");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Caught exception: {ex.Message}");
            }

            // Multiple async operations with error handling
            Console.WriteLine("\n2. Multiple operations error handling:");
            var tasks = new[]
            {
                OperationThatMightFail("Task A", false),
                OperationThatMightFail("Task B", true),   // This will fail
                OperationThatMightFail("Task C", false)
            };

            try
            {
                await Task.WhenAll(tasks);
                Console.WriteLine("All operations succeeded");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"One or more operations failed: {ex.Message}");
                
                // Check individual task results
                for (int i = 0; i < tasks.Length; i++)
                {
                    if (tasks[i].IsFaulted)
                    {
                        Console.WriteLine($"  Task {i} failed: {tasks[i].Exception?.InnerException?.Message}");
                    }
                    else if (tasks[i].IsCompletedSuccessfully)
                    {
                        Console.WriteLine($"  Task {i} succeeded");
                    }
                }
            }
        }

        static async Task<string> OperationThatMightFail(string operationName, bool shouldFail)
        {
            await Task.Delay(500);
            
            if (shouldFail)
                throw new InvalidOperationException($"{operationName} failed intentionally");
            
            return $"{operationName} completed successfully";
        }

        static async Task CancellationTokenDemo()
        {
            Console.WriteLine("--- Cancellation Token Demo ---");

            using var cts = new CancellationTokenSource();
            
            // Start a long-running task
            var longTask = LongRunningOperation("Long Task", cts.Token);
            
            Console.WriteLine("Long-running task started. Press any key to cancel...");
            Console.ReadKey();
            
            // Cancel the task
            cts.Cancel();
            
            try
            {
                await longTask;
                Console.WriteLine("Task completed successfully");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Task was cancelled");
            }

            // Demonstrate timeout-based cancellation
            Console.WriteLine("\n2. Timeout-based cancellation:");
            using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            
            try
            {
                await LongRunningOperation("Timeout Task", timeoutCts.Token);
                Console.WriteLine("Task completed within timeout");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Task was cancelled due to timeout");
            }
        }

        static async Task<string> LongRunningOperation(string taskName, CancellationToken cancellationToken)
        {
            Console.WriteLine($"{taskName} started");
            
            for (int i = 0; i < 10; i++)
            {
                // Check for cancellation
                cancellationToken.ThrowIfCancellationRequested();
                
                Console.WriteLine($"{taskName} - Step {i + 1}/10");
                await Task.Delay(1000, cancellationToken);
            }
            
            Console.WriteLine($"{taskName} completed");
            return $"{taskName} result";
        }

        static async Task ConfigureAwaitDemo()
        {
            Console.WriteLine("--- ConfigureAwait Demo ---");
            Console.WriteLine("This demonstrates the difference between ConfigureAwait(true) and ConfigureAwait(false)");
            Console.WriteLine("Note: The effect is more visible in GUI applications (WinForms/WPF)\n");

            Console.WriteLine($"Current thread: {Thread.CurrentThread.ManagedThreadId}");
            
            // With ConfigureAwait(true) - default behavior
            await OperationWithConfigureAwait("Operation 1", true);
            Console.WriteLine($"After ConfigureAwait(true): {Thread.CurrentThread.ManagedThreadId}");

            // With ConfigureAwait(false)
            await OperationWithConfigureAwait("Operation 2", false);
            Console.WriteLine($"After ConfigureAwait(false): {Thread.CurrentThread.ManagedThreadId}");

            Console.WriteLine("\nKey Points:");
            Console.WriteLine("• ConfigureAwait(false): Don't capture synchronization context");
            Console.WriteLine("• ConfigureAwait(true): Capture synchronization context (default)");
            Console.WriteLine("• Use ConfigureAwait(false) in library code for better performance");
        }

        static async Task OperationWithConfigureAwait(string operationName, bool continueOnCapturedContext)
        {
            Console.WriteLine($"{operationName} started on thread: {Thread.CurrentThread.ManagedThreadId}");
            
            await Task.Delay(1000).ConfigureAwait(continueOnCapturedContext);
            
            Console.WriteLine($"{operationName} continued on thread: {Thread.CurrentThread.ManagedThreadId}");
        }

        static async Task FileOperationsDemo()
        {
            Console.WriteLine("--- Async File Operations Demo ---");

            string fileName = "async_test.txt";
            string content = "This is a test file for async operations.\nLine 2\nLine 3\n";

            try
            {
                // Async file write
                Console.WriteLine("Writing file asynchronously...");
                await File.WriteAllTextAsync(fileName, content);
                Console.WriteLine($"File '{fileName}' written successfully");

                // Async file read
                Console.WriteLine("Reading file asynchronously...");
                string readContent = await File.ReadAllTextAsync(fileName);
                Console.WriteLine("File content:");
                Console.WriteLine(readContent);

                // Async file operations with streams
                Console.WriteLine("Appending to file using streams...");
                using (var stream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None, 4096, useAsync: true))
                using (var writer = new StreamWriter(stream))
                {
                    await writer.WriteLineAsync("This line was added asynchronously");
                }

                // Read all lines asynchronously
                Console.WriteLine("Reading all lines asynchronously...");
                string[] lines = await File.ReadAllLinesAsync(fileName);
                Console.WriteLine($"File contains {lines.Length} lines:");
                for (int i = 0; i < lines.Length; i++)
                {
                    Console.WriteLine($"  {i + 1}: {lines[i]}");
                }

                // Clean up
                File.Delete(fileName);
                Console.WriteLine("Cleanup completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File operation error: {ex.Message}");
            }
        }

        static async Task HttpClientDemo()
        {
            Console.WriteLine("--- HTTP Client Async Demo ---");

            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(10);

            try
            {
                // Single HTTP request
                Console.WriteLine("Making HTTP request to httpbin.org...");
                var sw = Stopwatch.StartNew();
                
                string response = await httpClient.GetStringAsync("https://httpbin.org/delay/2");
                
                sw.Stop();
                Console.WriteLine($"Response received in {sw.ElapsedMilliseconds}ms");
                Console.WriteLine($"Response length: {response.Length} characters");

                // Multiple concurrent HTTP requests
                Console.WriteLine("\nMaking multiple concurrent HTTP requests...");
                sw.Restart();

                string[] urls = 
                {
                    "https://httpbin.org/delay/1",
                    "https://httpbin.org/delay/1",
                    "https://httpbin.org/delay/1"
                };

                var tasks = urls.Select(url => httpClient.GetStringAsync(url)).ToArray();
                string[] responses = await Task.WhenAll(tasks);

                sw.Stop();
                Console.WriteLine($"All {responses.Length} requests completed in {sw.ElapsedMilliseconds}ms");
                Console.WriteLine($"Total response length: {responses.Sum(r => r.Length)} characters");

                // HTTP request with cancellation
                Console.WriteLine("\nMaking request with cancellation token...");
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                
                try
                {
                    string cancelResponse = await httpClient.GetStringAsync("https://httpbin.org/delay/3", cts.Token);
                    Console.WriteLine("Request completed successfully");
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Request was cancelled due to timeout");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP request error: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Request timeout: {ex.Message}");
            }
        }

        static async Task ProducerConsumerDemo()
        {
            Console.WriteLine("--- Producer-Consumer Pattern Demo ---");

            var buffer = new List<int>();
            var maxBufferSize = 5;
            var itemsToProcess = 20;
            var producerSemaphore = new SemaphoreSlim(maxBufferSize, maxBufferSize);
            var consumerSemaphore = new SemaphoreSlim(0, maxBufferSize);
            var bufferLock = new object();
            var random = new Random();

            // Producer task
            var producerTask = Task.Run(async () =>
            {
                for (int i = 1; i <= itemsToProcess; i++)
                {
                    await producerSemaphore.WaitAsync();
                    
                    lock (bufferLock)
                    {
                        buffer.Add(i);
                        Console.WriteLine($"Produced: {i}, Buffer size: {buffer.Count}");
                    }
                    
                    consumerSemaphore.Release();
                    await Task.Delay(random.Next(100, 500)); // Simulate variable production time
                }
                Console.WriteLine("Producer finished");
            });

            // Consumer task
            var consumerTask = Task.Run(async () =>
            {
                int consumed = 0;
                while (consumed < itemsToProcess)
                {
                    await consumerSemaphore.WaitAsync();
                    
                    int item;
                    lock (bufferLock)
                    {
                        item = buffer[0];
                        buffer.RemoveAt(0);
                        Console.WriteLine($"Consumed: {item}, Buffer size: {buffer.Count}");
                    }
                    
                    producerSemaphore.Release();
                    consumed++;
                    
                    // Simulate processing time
                    await Task.Delay(random.Next(200, 800));
                }
                Console.WriteLine("Consumer finished");
            });

            await Task.WhenAll(producerTask, consumerTask);
            Console.WriteLine("Producer-Consumer demo completed");
        }

        static async Task PerformanceComparisonDemo()
        {
            Console.WriteLine("--- Performance Comparison Demo ---");

            const int operationCount = 5;
            const int delayMs = 1000;

            // Synchronous execution
            Console.WriteLine("1. Synchronous execution:");
            var sw = Stopwatch.StartNew();
            
            for (int i = 0; i < operationCount; i++)
            {
                Thread.Sleep(delayMs);
                Console.WriteLine($"  Sync operation {i + 1} completed");
            }
            
            sw.Stop();
            Console.WriteLine($"Synchronous total time: {sw.ElapsedMilliseconds}ms\n");

            // Asynchronous sequential execution
            Console.WriteLine("2. Asynchronous sequential execution:");
            sw.Restart();
            
            for (int i = 0; i < operationCount; i++)
            {
                await Task.Delay(delayMs);
                Console.WriteLine($"  Async sequential operation {i + 1} completed");
            }
            
            sw.Stop();
            Console.WriteLine($"Async sequential total time: {sw.ElapsedMilliseconds}ms\n");

            // Asynchronous parallel execution
            Console.WriteLine("3. Asynchronous parallel execution:");
            sw.Restart();

            var parallelTasks = Enumerable.Range(1, operationCount)
                .Select(i => Task.Delay(delayMs).ContinueWith(_ => 
                    Console.WriteLine($"  Async parallel operation {i} completed")))
                .ToArray();

            await Task.WhenAll(parallelTasks);
            
            sw.Stop();
            Console.WriteLine($"Async parallel total time: {sw.ElapsedMilliseconds}ms");

            Console.WriteLine("\nPerformance Summary:");
            Console.WriteLine("• Synchronous: Blocks thread for each operation");
            Console.WriteLine("• Async Sequential: Doesn't block thread but operations are sequential");
            Console.WriteLine("• Async Parallel: Multiple operations run concurrently");
            Console.WriteLine("• Best approach depends on whether operations can be parallelized");
        }
    }
}
