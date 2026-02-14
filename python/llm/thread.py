from abc import ABC, abstractmethod
from typing import Generic, Optional, ParamSpec, TypeVar
import threading

P = ParamSpec('P')
R = TypeVar('R')

class Thread(threading.Thread, ABC, Generic[P, R]):
    def __init__(self, *args: P.args, **kwargs: P.kwargs) -> None:
        super().__init__(target=self._start, args=args, kwargs=kwargs)
        self._result: Optional[R] = None
        self._error: Optional[Exception] = None

    @property
    def result(self) -> R:
        super().join()
        if self._error: 
            raise self._error
        return self._result # type: ignore

    def _start(self, *args: P.args, **kwargs: P.kwargs) -> None:
        try:
            self._result = self.main(*args, **kwargs)
        except Exception as e:
            self._error = e

    @abstractmethod
    def main(self, *args: P.args, **kwargs: P.kwargs) -> R:
        """Abstract method that subclasses must implement."""
        pass
