import { EditorView, basicSetup } from "codemirror";
import { EditorState } from "@codemirror/state";
import { linter } from "@codemirror/lint";
import { mizan } from "./language.js";
import { clientLinter } from "./lint.js";
import { serverLinter } from "./apiLint.js";
import { autocompletion } from "@codemirror/autocomplete"; // <--- Add this
import { mizanAutocomplete } from "./autocomplete.js";
import { getEditorTheme } from "./theme.js";

/**
 * @param {Object} options 
 * @param {*} options.domElement the root element that the editor will be attached to
 * @param {string} [options.initialValue=""] initial code displayed in the text box
 * @param {(string) => void} [options.onChange=null] onChange event handler
 * @param {boolean} [options.isLightMode=true] use light or dark theme
 * @param {Array<Object>} [options.additionalExtentions] additional code mirror Extentions (linters, theme overrides)
 */
export function createMizanEditor({ domElement, initialValue = "",
  onChange = null, isLightMode = true,
  additionalExtentions = [], }) {
  const state = EditorState.create({
    doc: initialValue,
    extensions: [
      basicSetup,
      mizan(),
      autocompletion({ override: [mizanAutocomplete] }),
      linter(clientLinter),
      // debounce is configured directly in CM's linter function config
      //linter(serverLinter, { delay: 400 }),
      getEditorTheme(isLightMode),
      EditorView.updateListener.of((update) => {
        if (update.docChanged && onChange) {
          onChange(update.state.doc.toString());
        }
      }),
      additionalExtentions
    ]
  });

  const view = new EditorView({
    state,
    parent: domElement
  });

  // Returns a destroy method to easily wrap this inside a React useEffect cleanup
  return {
    view,
    destroy: () => view.destroy()
  };
}
