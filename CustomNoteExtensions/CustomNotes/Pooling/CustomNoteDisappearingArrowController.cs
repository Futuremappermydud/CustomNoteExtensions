using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CustomNoteExtensions.CustomNotes.Pooling
{
    public class CustomNoteDisappearingArrowController : DisappearingArrowControllerBase<CustomNoteGameNoteController>
    {

        protected override CustomNoteGameNoteController gameNoteController
        {
            get
            {
                return CustomNoteGameNoteController;
            }
        }



        public CustomNoteGameNoteController CustomNoteGameNoteController;
    }
}