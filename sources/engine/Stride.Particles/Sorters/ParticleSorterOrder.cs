// Copyright (c) Stride contributors (https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using Stride.Core.Mathematics;

namespace Stride.Particles.Sorters
{
    /// <summary>
    /// Sorts the particles by ascending order of their Order attribute
    /// </summary>
    public class ParticleSorterOrder : ParticleSorterCustom<uint>, IParticleSorter
    {
        public ParticleSorterOrder(ParticlePool pool) : base(pool, ParticleFields.Order) { }

        public unsafe ParticleList GetSortedList(Vector3 depth)
        {
            var livingParticles = ParticlePool.LivingParticles;

            var sortField = ParticlePool.GetField(fieldDesc);

            if (!sortField.IsValid())
            {
                // Field is not valid - return an unsorted list
                return new ParticleList(ParticlePool, livingParticles);
            }

            SortedParticle[] particleList = ArrayPool.Allocate(ParticlePool.ParticleCapacity);

            var i = 0;
            foreach (var particle in ParticlePool)
            {
                uint order = particle.Get(sortField);
                particleList[i] = new SortedParticle(particle, *((float*)(&order))*-1f);
                i++;
            }

            Array.Sort(particleList, 0, livingParticles);

            return new ParticleList(ParticlePool, livingParticles, particleList);
        }

        /// <summary>
        /// In case an array was used it must be freed back to the pool
        /// </summary>
        /// <param name="sortedList">Reference to the <see cref="ParticleList"/> to be freed</param>
        public void FreeSortedList(ref ParticleList sortedList)
        {
            sortedList.Free(ArrayPool);
        }
    }
}
