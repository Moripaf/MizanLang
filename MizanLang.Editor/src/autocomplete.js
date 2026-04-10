import { syntaxTree } from "@codemirror/language";
import { completeFromList } from "@codemirror/autocomplete";

// 1. Simple Keyword Completion
export const keywordCompletions = [
  "اگر", "باید", "یا", "و", "نیست", "نمیباشد", "در", "لیست", "بین",
  "صحیح", "غلط", "است", "باشد", "مقدار", "که"
].map(word => ({ label: word, type: "keyword" }));

const keywordSource = completeFromList(keywordCompletions);

// 2. Complex Context-Aware Completion (Dynamic)
async function dynamicCompletionSource(context) {
  const tree = syntaxTree(context.state);
  // Find the node just before the cursor
  const nodeBefore = tree.resolveInner(context.pos, -1);

  // Example: If typing inside a QuotedIdentifier e.g., [Pers...
  if (nodeBefore.name === "QuotedIdentifier" || nodeBefore.name === "BareIdentifier") {

    // Here you would do your API call or dynamic logic
    // const results = await fetch('/api/fields?search=' + context.matchBefore(/\w*/).text);

    return {
      from: nodeBefore.from,
      options: [
        { label: "Personnel.Age", type: "property" },
        { label: "Personnel.Salary", type: "property" }
      ]
    };
  }

  // Fallback to keyword completions if not in a special node
  const word = context.matchBefore(/\S*/);
  if (word.from === word.to && !context.explicit) return null;

  return {
    from: word.from,
    options: keywordCompletions
  };
}

export const mizanAutocomplete = dynamicCompletionSource;
