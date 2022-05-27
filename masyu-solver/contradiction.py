import view


class Contradiction(Exception):
    def __init__(self, *args: object) -> None:
        super().__init__(*args)
        view.log(str(self))
