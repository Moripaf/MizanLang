import { LRLanguage, LanguageSupport, HighlightStyle } from "@codemirror/language";
import { styleTags, tags as t } from "@lezer/highlight";
import { completeFromList } from "@codemirror/autocomplete"
import { parser } from "./parser.js";

// Add syntax highlighting via styleTags
export const mizanParser = parser.configure({
  props: [
    styleTags({
      "KwAgar KwBayad KwYa KwVa KwNist KwDar KwList KwBeyn": t.keyword,
      "Noise": t.comment,
      "BareIdentifier": t.variableName,
      "QuotedIdentifier": t.special(t.variableName),
      "String": t.string,
      "Number": t.number,
      "Boolean": t.bool,
      "CompareOp OpAdd OpSub OpMul OpDiv OpMod": t.operator,
      "Comment": t.lineComment,
      "( ) [ ]": t.paren,
      ", .": t.punctuation
    })
  ]
});

export const mizanLanguage = LRLanguage.define({
  parser: mizanParser,
  languageData: {
    commentTokens: { line: "//", block: { open: "/*", close: "*/" } }
  }
});
const mizanHighlightStyle = HighlightStyle.define([
  { tag: t.keyword, color: "#fff" },
  { tag: t.comment, color: "#f5d", fontStyle: "italic" }
])

export function mizan() {
  return new LanguageSupport(mizanLanguage);
}
