import { EditorView } from "@codemirror/view"
import { HighlightStyle, syntaxHighlighting } from "@codemirror/language"
import { tags as t } from "@lezer/highlight"

// Using https://github.com/one-dark/vscode-one-dark-theme/ as reference for the colors



// -------- DARK (One Dark) --------

const d_chalky = "#e5c07b",
  d_coral = "#e06c75",
  d_cyan = "#56b6c2",
  d_invalid = "#ffffff",
  d_ivory = "#abb2bf",
  d_stone = "#7d8799",
  d_malibu = "#61afef",
  d_sage = "#98c379",
  d_whiskey = "#d19a66",
  d_violet = "#c678dd",
  d_darkBackground = "#21252b",
  d_highlightBackground = "#2c313a",
  d_background = "#282c34",
  d_tooltipBackground = "#353a42",
  d_selection = "#3E4451",
  d_cursor = "#528bff";

const oneDarkTheme = EditorView.theme({
  "&": {
    color: d_ivory,
    backgroundColor: d_background,
    direction: "rtl",
    textAlign: "right",
    fontFamily: "Tahoma, Arial, sans-serif"
  },

  ".cm-content": { caretColor: d_cursor },
  ".cm-cursor, .cm-dropCursor": { borderLeftColor: d_cursor },

  "&.cm-focused > .cm-scroller > .cm-selectionLayer .cm-selectionBackground, .cm-selectionBackground, .cm-content ::selection": {
    backgroundColor: d_selection
  },

  ".cm-panels": { backgroundColor: d_darkBackground, color: d_ivory },
  ".cm-panels.cm-panels-top": { borderBottom: "2px solid black" },
  ".cm-panels.cm-panels-bottom": { borderTop: "2px solid black" },

  ".cm-searchMatch": {
    backgroundColor: "#72a1ff59",
    outline: "1px solid #457dff"
  },
  ".cm-searchMatch.cm-searchMatch-selected": {
    backgroundColor: "#6199ff2f"
  },

  ".cm-activeLine": { backgroundColor: "#6699ff0b" },
  ".cm-selectionMatch": { backgroundColor: "#aafe661a" },

  "&.cm-focused .cm-matchingBracket, &.cm-focused .cm-nonmatchingBracket": {
    backgroundColor: "#bad0f847"
  },

  ".cm-gutters": {
    backgroundColor: d_background,
    color: d_stone,
    border: "none"
  },

  ".cm-activeLineGutter": {
    backgroundColor: d_highlightBackground
  },

  ".cm-foldPlaceholder": {
    backgroundColor: "transparent",
    border: "none",
    color: "#ddd"
  },

  ".cm-tooltip": {
    border: "none",
    backgroundColor: d_tooltipBackground
  },
  ".cm-tooltip .cm-tooltip-arrow:before": {
    borderTopColor: "transparent",
    borderBottomColor: "transparent"
  },
  ".cm-tooltip .cm-tooltip-arrow:after": {
    borderTopColor: d_tooltipBackground,
    borderBottomColor: d_tooltipBackground
  },

  ".cm-tooltip-autocomplete": {
    "& > ul > li[aria-selected]": {
      backgroundColor: d_highlightBackground,
      color: d_ivory
    }
  }
}, { dark: true });

const oneDarkHighlightStyle = HighlightStyle.define([
  { tag: t.keyword, color: d_violet },
  { tag: [t.name, t.deleted, t.character, t.propertyName, t.macroName], color: d_coral },
  { tag: [t.function(t.variableName), t.labelName], color: d_malibu },
  { tag: [t.color, t.constant(t.name), t.standard(t.name)], color: d_whiskey },
  { tag: [t.definition(t.name), t.separator], color: d_ivory },
  { tag: [t.typeName, t.className, t.number, t.changed, t.annotation, t.modifier, t.self, t.namespace], color: d_chalky },
  { tag: [t.operator, t.operatorKeyword, t.url, t.escape, t.regexp, t.link, t.special(t.string)], color: d_cyan },
  { tag: [t.meta, t.comment], color: d_stone },
  { tag: t.strong, fontWeight: "bold" },
  { tag: t.emphasis, fontStyle: "italic" },
  { tag: t.strikethrough, textDecoration: "line-through" },
  { tag: t.link, color: d_stone, textDecoration: "underline" },
  { tag: t.heading, fontWeight: "bold", color: d_coral },
  { tag: [t.atom, t.bool, t.special(t.variableName)], color: d_whiskey },
  { tag: [t.processingInstruction, t.string, t.inserted], color: d_sage },
  { tag: t.invalid, color: d_invalid }
]);

const oneDark = [oneDarkTheme, syntaxHighlighting(oneDarkHighlightStyle)];



// -------- LIGHT (One Light) --------

const l_chalky = "#b76b00";
const l_coral = "#d14";
const l_cyan = "#0184bc";
const l_invalid = "#ff0000";
const l_ivory = "#383a42";
const l_stone = "#6a6a6a";
const l_malibu = "#4078f2";
const l_sage = "#50a14f";
const l_whiskey = "#986801";
const l_violet = "#a626a4";

const l_darkBackground = "#eaeaea";
const l_highlightBackground = "#f2f2f2";
const l_background = "#fafafa";
const l_tooltipBackground = "#f0f0f0";
const l_selection = "#cce2ff";
const l_cursor = "#526fff";

const oneLightTheme = EditorView.theme({
  "&": {
    color: l_ivory,
    backgroundColor: l_background,
    direction: "rtl",
    textAlign: "right",
    fontFamily: "Tahoma, Arial, sans-serif"
  },

  ".cm-content": { caretColor: l_cursor },
  ".cm-cursor, .cm-dropCursor": { borderLeftColor: l_cursor },

  "&.cm-focused > .cm-scroller > .cm-selectionLayer .cm-selectionBackground, .cm-selectionBackground, .cm-content ::selection": {
    backgroundColor: l_selection
  },

  ".cm-panels": { backgroundColor: l_darkBackground, color: l_ivory },
  ".cm-panels.cm-panels-top": { borderBottom: "2px solid #ccc" },
  ".cm-panels.cm-panels-bottom": { borderTop: "2px solid #ccc" },

  ".cm-searchMatch": {
    backgroundColor: "#aaddff80",
    outline: "1px solid #77aaff"
  },
  ".cm-searchMatch.cm-searchMatch-selected": {
    backgroundColor: "#88b7ff55"
  },

  ".cm-activeLine": { backgroundColor: "#dfe8ff80" },
  ".cm-selectionMatch": { backgroundColor: "#d8f5b580" },

  "&.cm-focused .cm-matchingBracket, &.cm-focused .cm-nonmatchingBracket": {
    backgroundColor: "#cce0ff80"
  },

  ".cm-gutters": {
    backgroundColor: l_background,
    color: l_stone,
    border: "none"
  },

  ".cm-activeLineGutter": { backgroundColor: l_highlightBackground },

  ".cm-foldPlaceholder": {
    backgroundColor: "transparent",
    border: "none",
    color: "#777"
  },

  ".cm-tooltip": {
    border: "1px solid #ccc",
    backgroundColor: l_tooltipBackground
  },
  ".cm-tooltip .cm-tooltip-arrow:before": {
    borderTopColor: "transparent",
    borderBottomColor: "transparent"
  },
  ".cm-tooltip .cm-tooltip-arrow:after": {
    borderTopColor: l_tooltipBackground,
    borderBottomColor: l_tooltipBackground
  },

  ".cm-tooltip-autocomplete": {
    "& > ul > li[aria-selected]": {
      backgroundColor: l_highlightBackground,
      color: l_ivory
    }
  }
}, { dark: false });

const oneLightHighlightStyle = HighlightStyle.define([
  { tag: t.keyword, color: l_violet },
  { tag: [t.name, t.deleted, t.character, t.propertyName, t.macroName], color: l_coral },
  { tag: [t.function(t.variableName), t.labelName], color: l_malibu },
  { tag: [t.color, t.constant(t.name), t.standard(t.name)], color: l_whiskey },
  { tag: [t.definition(t.name), t.separator], color: l_ivory },
  { tag: [t.typeName, t.className, t.number, t.changed, t.annotation, t.modifier, t.self, t.namespace], color: l_chalky },
  { tag: [t.operator, t.operatorKeyword, t.url, t.escape, t.regexp, t.link, t.special(t.string)], color: l_cyan },
  { tag: [t.meta, t.comment], color: l_stone },
  { tag: t.strong, fontWeight: "bold" },
  { tag: t.emphasis, fontStyle: "italic" },
  { tag: t.strikethrough, textDecoration: "line-through" },
  { tag: t.link, color: l_stone, textDecoration: "underline" },
  { tag: t.heading, fontWeight: "bold", color: l_coral },
  { tag: [t.atom, t.bool, t.special(t.variableName)], color: l_whiskey },
  { tag: [t.processingInstruction, t.string, t.inserted], color: l_sage },
  { tag: t.invalid, color: l_invalid }
]);

const oneLight = [oneLightTheme, syntaxHighlighting(oneLightHighlightStyle)];


// -------- RETURN --------

// Function that returns CodeMirror theme based on a boolean.
// true  -> light mode
// false -> dark mode
export function getEditorTheme(isLight) {
  return isLight ? oneLight : oneDark;
}
