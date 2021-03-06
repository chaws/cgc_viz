// Copyright 2017 voidALPHA, Inc.
// This file is part of the Haxxis video generation system and is provided
// by voidALPHA in support of the Cyber Grand Challenge.
// Haxxis is free software: you can redistribute it and/or modify it under the terms
// of the GNU General Public License as published by the Free Software Foundation.
// Haxxis is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
// PARTICULAR PURPOSE. See the GNU General Public License for more details.
// You should have received a copy of the GNU General Public License along with
// Haxxis. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utility;
using Visualizers;

namespace Mutation.Mutators.BoundManipulationMutators
{
    public class RelativePositionBoundMutator : Mutator
    {
        private MutableField<Vector3> m_ScaledOffset = new MutableField<Vector3>()
        {LiteralValue = Vector3.right};

        [Controllable(LabelText = "Offset from Parent")]
        public MutableField<Vector3> ScaledOffset
        {
            get { return m_ScaledOffset; }
        }

        private MutableField<string> m_BoundName = new MutableField<string>() 
        { LiteralValue = "" };
        [Controllable(LabelText = "Bound Name Override")]
        public MutableField<string> BoundName { get { return m_BoundName; } }


        public override IEnumerator ReceivePayload(VisualPayload payload)
        {
            BoundingBox newBound = payload.VisualData.Bound.CreateDependingBound(Name);

            var nameOverride = BoundName.GetFirstValue( payload.Data );

            newBound.name = nameOverride=="" ? Name : nameOverride;

            newBound.transform.position +=
                payload.VisualData.Bound.transform.PiecewiseMultiply(
                ScaledOffset.GetFirstValue(payload.Data));

            var newPayload = new VisualPayload(payload.Data, new VisualDescription(newBound));

            var iterator = Router.TransmitAll(newPayload);
            while (iterator.MoveNext())
                yield return null;
        }
    }
}
