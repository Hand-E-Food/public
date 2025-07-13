import { createInterface, Readline } from 'readline/promises';

const i = createInterface(process.stdin, process.stdout, undefined, true);
const r = new Readline(process.stdout);

export const Console = {
    clearLine: r.clearLine.bind(r),
    close: i.close.bind(i),
    commit: r.commit.bind(r),
    cursorTo: r.cursorTo.bind(r),
    getCursorPos: i.getCursorPos.bind(i),
    moveCursor: r.moveCursor.bind(r),
    question: i.question.bind(i),
    write: i.write.bind(i),
};
