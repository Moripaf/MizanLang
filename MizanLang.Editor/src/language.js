import { LRLanguage, LanguageSupport } from "@codemirror/language";
import { styleTags, tags as t } from "@lezer/highlight";
import { parser } from "./parser.js";

// Add syntax highlighting via styleTags
export const dslParser = parser.configure({
  props: [
    styleTags({
      "kwAgar kwBayad kwYa kwVa kwNist kwDar kwList kwBeyn": t.keyword,
      "Noise": t.modifier,
      "BareIdentifier": t.variableName,
      "QuotedIdentifier": t.special(t.variableName),
      "String": t.string,
      "Number": t.number,
      "Boolean": t.bool,
      "CompareOp opAdd opSub opMul opDiv opMod": t.operator,
      "Comment": t.lineComment,
      "( ) [ ]": t.paren,
      ", .": t.punctuation
    })
  ]
});

export const dslLanguage = LRLanguage.define({
  parser: dslParser,
  languageData: {
    commentTokens: { line: "//", block: { open: "/*", close: "*/" } }
  }
});

export function dsl() {
  return new LanguageSupport(dslLanguage);
}
