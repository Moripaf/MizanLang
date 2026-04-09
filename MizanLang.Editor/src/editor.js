import { EditorView, basicSetup } from "codemirror";
import { EditorState } from "@codemirror/state";
import { linter, lintGutter } from "@codemirror/lint";
import { dsl } from "./language.js";
import { clientLinter } from "./lint.js";
import { serverLinter } from "./apiLint.js";

export function createDslEditor(domElement, initialValue = "", onChange = null) {
  const state = EditorState.create({
    doc: initialValue,
    extensions: [
      basicSetup,
      dsl(),
      lintGutter(),
      linter(clientLinter),
      // debounce is configured directly in CM's linter function config
      linter(serverLinter, { delay: 400 }),
      EditorView.theme({
        "&": { height: "100%", direction: "rtl", textAlign: "right", fontFamily: "Tahoma, Arial, sans-serif" },
        ".cm-scroller": { overflow: "auto" }
      }, { dark: true }),
      EditorView.updateListener.of((update) => {
        if (update.docChanged && onChange) {
          onChange(update.state.doc.toString());
        }
      })
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
