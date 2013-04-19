//
//  Copyright 2013, Desert Software Solutions Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace DesertSoftware.Web
{
    public abstract class BackgroundWorker
    {
        private const int DEFAULT_FREQUENCY = 10;   // Default frequency to run every ten minutes

        private string key;
        private TimeSpan frequency;
        private bool firstTime = true;

        /// <summary>
        /// Registers a background worker in the work queue.
        /// </summary>
        /// <param name="worker">The worker to be registered.</param>
        /// <returns></returns>
        static public BackgroundWorker RegisterWorker(BackgroundWorker worker) {
            if (HttpRuntime.Cache[worker.key] == null) {
                TimeSpan expiration = worker.frequency;

                if (worker.firstTime) {
                    worker.firstTime = false;

                    // force worker frequency to 1 minute if this is the initial startup
                    expiration = TimeSpan.FromMinutes(1);
                }

                HttpRuntime.Cache.Add(worker.key, worker, null, DateTime.MaxValue, expiration, CacheItemPriority.Normal,
                    new CacheItemRemovedCallback(worker.Start));
            }

            return worker;
        }

        // The call back that will be executed when we are removed from the cache
        private void Start(string key, object value, CacheItemRemovedReason reason) {

            // If we haven't been removed due to cache expiration then assume it's time to quit
            if (reason != CacheItemRemovedReason.Expired) 
                return;

            try {
                // Delegate to the run method
                Run();
            } finally {
                // Register this instance into the work queue
                RegisterWorker(this);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundWorker" /> class.
        /// </summary>
        public BackgroundWorker() 
            : this(DEFAULT_FREQUENCY) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundWorker" /> class.
        /// </summary>
        /// <param name="frequency">The frequency in minutes that this worker should run.</param>
        public BackgroundWorker(int frequency) {
            this.frequency = TimeSpan.FromMinutes(frequency);
            this.key = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        /// <value>
        /// The frequency in minutes.
        /// </value>
        public int Frequency {
            get { return (int)this.frequency.TotalMinutes; }
            set { this.frequency = TimeSpan.FromMinutes(value); }
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        protected abstract void Run();
    }
}
