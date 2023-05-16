using BeatSaberMarkupLanguage;
using IPA.Utilities;
using UnityEngine;
using Zenject;

namespace CustomNoteExtensions.CustomNotes.Pooling
{
    public class CustomNoteNoteObjectsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var beatmapObjectsInstaller = FindObjectOfType<BeatmapObjectsInstaller>();
            var cube = GetBasicNoteObject(beatmapObjectsInstaller.GetField<GameNoteController, BeatmapObjectsInstaller>("_normalBasicNotePrefab").gameObject);

            base.Container.BindMemoryPool<CustomNoteGameNoteController, CustomNoteGameNoteController.Pool>().WithId(NoteData.GameplayType.Normal).WithInitialSize(25).FromComponentInNewPrefab(cube);
            base.Container.Bind<CustomNoteBeatmapObjectManager>().To<CustomNoteBeatmapObjectManager>().AsSingle().NonLazy();

        }

        internal GameObject GetBasicNoteObject(GameObject normalNote)
        {
            var cube = GameObject.Instantiate(normalNote);
            cube.SetActive(false);
            cube.name = "CustomNoteGameNoteController";
            var noteCon = cube.AddComponent<CustomNoteGameNoteController>();

			var controller = cube.GetComponent<GameNoteController>();

            var noteCube = cube.transform.Find("NoteCube");

            var oldDisArrowCon = cube.GetComponent<DisappearingArrowController>();
            var disArrowCon = cube.AddComponent<CustomNoteDisappearingArrowController>();
            disArrowCon.CustomNoteGameNoteController = noteCon;
            if (controller != null)
            {

                disArrowCon.SetField<DisappearingArrowControllerBase<CustomNoteGameNoteController>, CutoutEffect>("_arrowCutoutEffect", oldDisArrowCon.GetField<CutoutEffect, DisappearingArrowControllerBase<GameNoteController>>("_arrowCutoutEffect"));
                disArrowCon.SetField<DisappearingArrowControllerBase<CustomNoteGameNoteController>, MeshRenderer>("_cubeMeshRenderer", oldDisArrowCon.GetField<MeshRenderer, DisappearingArrowControllerBase<GameNoteController>>("_cubeMeshRenderer"));
                disArrowCon.SetField<DisappearingArrowControllerBase<CustomNoteGameNoteController>, MaterialPropertyBlockController[]>("_transparentObjectMaterialPropertyBlocks", oldDisArrowCon.GetField<MaterialPropertyBlockController[], DisappearingArrowControllerBase<GameNoteController>>("_transparentObjectMaterialPropertyBlocks"));
                DestroyImmediate(oldDisArrowCon);

            }
            else
            {
                var oldburstDisArrowCon = cube.GetComponent<BurstSliderNoteDisappearingArrowController>();

                disArrowCon.SetField<DisappearingArrowControllerBase<CustomNoteGameNoteController>, CutoutEffect>("_arrowCutoutEffect", oldburstDisArrowCon.GetField<CutoutEffect, DisappearingArrowControllerBase<BurstSliderGameNoteController>>("_arrowCutoutEffect"));
                disArrowCon.SetField<DisappearingArrowControllerBase<CustomNoteGameNoteController>, MeshRenderer>("_cubeMeshRenderer", oldburstDisArrowCon.GetField<MeshRenderer, DisappearingArrowControllerBase<BurstSliderGameNoteController>>("_cubeMeshRenderer"));
                disArrowCon.SetField<DisappearingArrowControllerBase<CustomNoteGameNoteController>, MaterialPropertyBlockController[]>("_transparentObjectMaterialPropertyBlocks", oldburstDisArrowCon.GetField<MaterialPropertyBlockController[], DisappearingArrowControllerBase<BurstSliderGameNoteController>>("_transparentObjectMaterialPropertyBlocks"));
                DestroyImmediate(oldburstDisArrowCon);

            }

            noteCon.InitializeFromOld(controller.noteMovement, controller.GetField<BoxCuttableBySaber[], GameNoteController>("_smallCuttableBySaberList"), controller.GetField<BoxCuttableBySaber[], GameNoteController>("_bigCuttableBySaberList"), controller.GetField<GameObject, GameNoteController>("_wrapperGO"), noteCube);
            foreach(NoteBigCuttableColliderSize colliderSize in noteCon.GetComponentsInChildren<NoteBigCuttableColliderSize>())
            {
                colliderSize.SetField<NoteBigCuttableColliderSize, NoteController>("_noteController", noteCon);
            }

            var colorNoteVisuals = cube.GetComponent<ColorNoteVisuals>();
			var customNoteVisuals = cube.AddComponent<CustomColorNoteVisuals>();
			customNoteVisuals._arrowMeshRenderers = colorNoteVisuals.GetField<MeshRenderer[], ColorNoteVisuals>("_arrowMeshRenderers");
            customNoteVisuals._circleMeshRenderers = colorNoteVisuals.GetField<MeshRenderer[], ColorNoteVisuals>("_circleMeshRenderers");
            customNoteVisuals._materialPropertyBlockControllers = colorNoteVisuals.GetField<MaterialPropertyBlockController[], ColorNoteVisuals>("_materialPropertyBlockControllers");
            customNoteVisuals._noteController = colorNoteVisuals.GetField<NoteControllerBase, ColorNoteVisuals>("_noteController");
            customNoteVisuals._defaultColorAlpha = colorNoteVisuals.GetField<float, ColorNoteVisuals>("_defaultColorAlpha");

			DestroyImmediate(controller);
            DestroyImmediate(colorNoteVisuals);

			var baseNotevisuals = cube.GetComponent<BaseNoteVisuals>();
			baseNotevisuals.SetField<BaseNoteVisuals, NoteControllerBase>("_noteController", noteCon);
			colorNoteVisuals.SetField<ColorNoteVisuals, NoteControllerBase>("_noteController", noteCon);

			return cube;

        }
    }
}