﻿using CefSharp;
using CefSharp.WinForms;
using Sentry;
using System;
using System.Windows.Forms;

namespace HiTechBrowser
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Init the Sentry SDK
            SentrySdk.Init(o =>
            {
                // Tells which project in Sentry to send events to:
                o.Dsn = "https://ee49488116c8457eb97d44ef504295b3@sentry.cowr.org/7";
                // When configuring for the first time, to see what the SDK is doing:
                o.Debug = true;
                // Set TracesSampleRate to 1.0 to capture 100% of transactions for performance monitoring.
                // We recommend adjusting this value in production.
                o.TracesSampleRate = 1.0;
            });
            // Configure WinForms to throw exceptions so Sentry can capture them.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);

            // Transaction can be started by providing, at minimum, the name and the operation
            var transaction = SentrySdk.StartTransaction(
              "test-transaction-name",
              "test-transaction-operation"
            );

            // Transactions can have child spans (and those spans can have child spans as well)
            var span = transaction.StartChild("test-child-operation");

            // ...
            // (Perform the operation represented by the span/transaction)
            // ...

            span.Finish(); // Mark the span as finished
            transaction.Finish(); // Mark the transaction as finished and send it to Sentry

            // Initialize CefSharp if not already initialized
            if (!Cef.IsInitialized) // Добавил проверку
            {
                Cef.Initialize(new CefSettings()); // Добавил аргумент
            }

            // Parse the command line argument for the link
            string link = "https://hitech.pro100byte.ru"; // Default link
            if (args.Length > 0 && args[0].StartsWith("--link="))
            {
                link = args[0].Substring(7); // Remove the --link= part
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(link)); // Pass the link to the form constructor
        }
    }
}
