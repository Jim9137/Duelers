// using System;
// using UnityEngine;

// namespace Duelers.Local.Controller
// {
//     public class SelectionController
//     {
//         private GameObject _currentSelected;

//         private MouseState _currentState;

//         public void SelectObject(GameObject obj)
//         {
//             switch (_currentState)
//             {
//                 case MouseState.UNITSELECTED:
//                 case MouseState.NOTHING:
//                     _currentState = MouseState.UNITSELECTED;
//                     _currentSelected = obj;
//                     break;
//                 default:
//                     _currentState = MouseState.NOTHING;
//                     _currentSelected = null;
//                     break;
//             }
//         }


//         public void SelectEmptyTile()
//         {
//             switch (_currentState)
//             {
//                 case MouseState.NOTHING:
//                     break;
//                 case MouseState.UNITSELECTED:

//                     break;
//                 default:
//                     throw new ArgumentOutOfRangeException();
//             }

//             _currentState = MouseState.NOTHING;
//             _currentSelected = null;
//         }

//         public GameObject GetActiveObject() => _currentSelected;

//         private enum MouseState
//         {
//             NOTHING,
//             UNITSELECTED
//         }
//     }
// }