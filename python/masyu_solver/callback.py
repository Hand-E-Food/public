class Callback:
    def __init__(self, function, *args):
        self.function = function
        self.args = args
    
    def __call__(self, *args):
        return self.function(*self.args)
