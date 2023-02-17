﻿namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of a container for systems.
    /// </summary>
    public interface ISystems
    {
        /// <summary>
        /// Adds the system.
        /// </summary>
        ISystems Add(ISystem system);

        /// <summary>
        /// Creates and adds a system of type <typeparamref name="TSystem"/>
        /// </summary>
        ISystems Add<TSystem>() where TSystem : class, ISystem, new();

        /// <summary>
        /// Initialize all the required systems.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Starts up all the required systems.
        /// </summary>
        void StartUp();

        /// <summary>
        /// Executes all the required systems.
        /// </summary>
        void Execute();

        /// <summary>
        /// An interface for storing systems' container in another container.
        /// </summary>
        public interface IForContainer
        {
            /// <summary>
            /// Disposes all the systems before deleting.
            /// </summary>
            void Dispose();
        }
    }
}