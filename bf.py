import collections
import sys

commands = "<>[]+-.,"

def strip_comments(source):
    return filter(lambda c: c in commands, source)


def get_matching_brackets(source, b_open, b_close):

    stack, open_matches = [], {}
    for i, c in enumerate(source):
        if c == b_open:
            stack.append(i)
        elif c == b_close:
            try:
                open_matches[stack.pop()] = i
            except IndexError:
                raise ValueError("Too many closed brackets")

    if stack:  # check if stack is empty afterwards
        raise ValueError("Too many opening parentheses")
    else:
        close_matches = {v: k for k, v in open_matches.iteritems()}
        return open_matches, close_matches


class Interpreter:

    def __init__(self):
        self.memory = collections.defaultdict(lambda: 0)
        self.data_pointer = 0
        self.instruction_pointer = 0
        self.source = None
        self.open_matches = self.close_matches = {}

    def run(self, source=None):

        self.source = strip_comments(source) if source else []
        self.open_matches, self.close_matches = get_matching_brackets(self.source, '[', ']')

        while self.instruction_pointer < len(self.source):
            self._do_cmd(self.source[self.instruction_pointer])
            self.instruction_pointer += 1

    def _do_cmd(self, cmd):

        if cmd == ">":
            self.data_pointer += 1
        if cmd == "<":
            self.data_pointer -= 1

        if cmd == "+":
            self.memory[self.data_pointer] += 1
        if cmd == "-":
            self.memory[self.data_pointer] -= 1

        if cmd == ".":
            sys.stdout.write(chr(self.memory[self.data_pointer]))
        if cmd == ",":
            self.memory[self.data_pointer] = ord(sys.stdin.read(1))

        if cmd == "[":
            if self.memory[self.data_pointer]:
                pass
            else:
                self.instruction_pointer = self.open_matches[self.instruction_pointer]

        if cmd == "]":
            if self.memory[self.data_pointer]:
                self.instruction_pointer = self.close_matches[self.instruction_pointer]
            else:
                pass


if __name__ == '__main__':
    source = """++++[>+++++<-]>[<+++++>-]+<+[
    >[>+>+<<-]++>>[<<+>>-]>>>[-]++>[-]+
    >>>+[[-]++++++>>>]<<<[[<++++++++<++>>-]+<.<[>----<-]<]
    <<[>>>>>[>>>[-]+++++++++<[>-<-]+++++++++>[-[<->-]+[<<<]]<[>+<-]>]<<-]<<-
]
[Outputs square numbers from 0 to 10000.
Daniel B Cristofani (cristofdathevanetdotcom)
http://www.hevanet.com/cristofd/brainfuck/]
    """

    i = Interpreter()
    i.run(source)
